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
using Windows.ApplicationModel.DataTransfer;
using OneDo.ViewModel.Core;
using OneDo.ViewModel.Core.Command;

namespace OneDo.ViewModel
{
    public class LogViewModel : ModalViewModel
    {
        private List<string> items;
        public List<string> Items
        {
            get { return items; }
            set { Set(ref items, value); }
        }

        private List<string> selectedItems;
        public List<string> SelectedItems
        {
            get { return selectedItems; }
            set { Set(ref selectedItems, value); }
        }

        public ICommand LoadCommand { get; }

        public ICommand ExportCommand { get; }

        public ICommand CopyToClipboardCommand { get; }

        public ICommand ClearCommand { get; }

        public IProgressService ProgressService { get; }

        public LogViewModel(IProgressService progressService)
        {
            ProgressService = progressService;
            LoadCommand = new AsyncRelayCommand(Load);
            ExportCommand = new AsyncRelayCommand(Export);
            CopyToClipboardCommand = new RelayCommand(CopyToClipboard);
            ClearCommand = new AsyncRelayCommand(Clear);
        }

        public async Task Load()
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
                        Items = Regex.Split(logText, @"\r?\n").ToList();
                    }

                    var memoryLogger = Logger.Current as MemoryLogger;
                    if (memoryLogger != null)
                    {
                        Items = memoryLogger.Items.ToList();
                    }
                }
                catch
                {
                    Items = new List<string>();
                }
            });
        }

        public async Task Export()
        {
            await ProgressService.RunAsync(async () =>
            {
                var logText = string.Join(Environment.NewLine, Items ?? new List<string>());
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

        public void CopyToClipboard()
        {
            var items = (SelectedItems?.Any() ?? false) ? SelectedItems : Items;
            var logText = string.Join(Environment.NewLine, items ?? new List<string>());
            try
            {
                var dataPackage = new DataPackage();
                dataPackage.RequestedOperation = DataPackageOperation.Copy;
                dataPackage.SetText(logText);

                Clipboard.SetContent(dataPackage);
                Clipboard.Flush();
            }
            catch (Exception e)
            {
                Logger.Current.Error(e);
            }
        }

        private async Task Clear()
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
                catch (Exception e)
                {
                    Logger.Current.Error("Log clear", e);
                }
                finally
                {
                    Logger.Current.Info("Log was cleared");
                    await Load();
                }
            });
        }
    }
}
