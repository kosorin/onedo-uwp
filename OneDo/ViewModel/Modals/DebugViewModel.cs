using OneDo.Common.Logging;
using OneDo.Model.Data;
using OneDo.Services.ModalService;
using OneDo.Services.ProgressService;
using OneDo.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;

namespace OneDo.ViewModel.Modals
{
    public class DebugViewModel : ModalViewModel
    {
        private string log;
        public string Log
        {
            get { return log; }
            set { Set(ref log, value); }
        }

        public ICommand ClearLogCommand { get; }

        public IProgressService ProgressService { get; }

        public DebugViewModel(IModalService modalService, ISettingsProvider settingsProvider, IProgressService progressService) : base(modalService, settingsProvider)
        {
            ProgressService = progressService;
            ClearLogCommand = new AsyncRelayCommand(ClearLog);
        }

        public async Task LoadLog()
        {
            await ProgressService.RunAsync(async () =>
            {
                var logger = Logger.Current as FileLogger;
                if (logger != null)
                {
                    try
                    {
                        var folder = ApplicationData.Current.LocalFolder;
                        var path = logger.Path.Replace(folder.Path, "");
                        var file = await folder.CreateFileAsync(path, CreationCollisionOption.OpenIfExists);
                        Log = await FileIO.ReadTextAsync(file);
                    }
                    catch
                    {
                        Log = "Failed to load log";
                    }
                }
            });
        }

        private async Task ClearLog()
        {
            await ProgressService.RunAsync(async () =>
            {
                var logger = Logger.Current as FileLogger;
                if (logger != null)
                {
                    try
                    {
                        var folder = ApplicationData.Current.LocalFolder;
                        var path = logger.Path.Replace(folder.Path, "");
                        var file = await folder.CreateFileAsync(path, CreationCollisionOption.ReplaceExisting);
                        Log = "";
                    }
                    catch { }
                }
            });
        }
    }
}
