using OneDo.Common.Logging;
using OneDo.Services.ProgressService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.Storage.Pickers;
using OneDo.Common.Extensions;
using OneDo.Common.Mvvm;

namespace OneDo.ViewModel
{
    public class DebugViewModel : ModalViewModel
    {
        private List<string> log;
        public List<string> Log
        {
            get { return log; }
            set { Set(ref log, value); }
        }

        public ICommand LoadLogCommand { get; }

        public ICommand ExportLogCommand { get; }

        public ICommand ClearLogCommand { get; }

        public IProgressService ProgressService { get; }

        public DebugViewModel(IProgressService progressService)
        {
            ProgressService = progressService;
            LoadLogCommand = new AsyncRelayCommand(LoadLog);
            ExportLogCommand = new AsyncRelayCommand(ExportLog);
            ClearLogCommand = new AsyncRelayCommand(ClearLog);

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            LoadLog();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        public async Task LoadLog()
        {
            await ProgressService.RunAsync(async () =>
            {
                try
                {
                    var fileLogger = Logger.Current as FileLogger;
                    if (fileLogger != null)
                    {
                        var folder = ApplicationData.Current.LocalFolder;
                        var path = fileLogger.Path.Replace(folder.Path, "");
                        var file = await folder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
                        var logText = await FileIO.ReadTextAsync(file);
                        Log = Regex.Split(logText, @"\r?\n").ToList();
                    }
                    var memoryLogger = Logger.Current as MemoryLogger;
                    if (memoryLogger != null)
                    {
                        Log = memoryLogger.Items.ToList();
                    }
                }
                catch
                {
                    Log = new List<string>();
                }
            });
        }

        public async Task ExportLog()
        {
            await ProgressService.RunAsync(async () =>
            {
                var logText = string.Join(Environment.NewLine, Log ?? new List<string>());

                try
                {
                    var picker = new FileSavePicker();
                    picker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                    picker.SuggestedFileName = $"Log_{DateTime.Now.ToFileName()}.txt";
                    picker.FileTypeChoices.Add("Text", new List<string>() { ".txt" });

                    var file = await picker.PickSaveFileAsync();
                    if (file != null)
                    {
                        await FileIO.WriteTextAsync(file, logText);
                    }
                }
                catch (Exception e)
                {
                    Logger.Current.Error(e);
                }
            });
        }

        private async Task ClearLog()
        {
            await ProgressService.RunAsync(async () =>
            {
                try
                {
                    var fileLogger = Logger.Current as FileLogger;
                    if (fileLogger != null)
                    {

                        var folder = ApplicationData.Current.LocalFolder;
                        var path = fileLogger.Path.Replace(folder.Path, "");
                        var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                    }
                    var memoryLogger = Logger.Current as MemoryLogger;
                    if (memoryLogger != null)
                    {
                        memoryLogger.Items.Clear();
                    }
                }
                catch { }
                finally
                {
                    Log = new List<string>();
                }
            });
        }
    }
}
