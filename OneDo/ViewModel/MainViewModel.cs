using OneDo.Services.ModalService;
using System.Windows.Input;
using OneDo.Model.Data;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Core;
using OneDo.Model.Data.Entities;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using OneDo.ViewModel.Commands;
using OneDo.Services.ProgressService;
using System;
using Microsoft.Band;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Band.Tiles;
using OneDo.Band;
using Microsoft.Band.Tiles.Pages;
using OneDo.Common.Logging;

namespace OneDo.ViewModel
{
    public class MainViewModel : ExtendedViewModel
    {
        private FolderListViewModel folderList;
        public FolderListViewModel FolderList
        {
            get { return folderList; }
            set { Set(ref folderList, value); }
        }

        private NoteListViewModel noteList;
        public NoteListViewModel NoteList
        {
            get { return noteList; }
            set { Set(ref noteList, value); }
        }


        public ICommand ShowSettingsCommand { get; }

        public IModalService ModalService { get; }

        public DataService DataService { get; }

        public IProgressService ProgressService { get; }

        public MainViewModel(IModalService modalService, DataService dataService, IProgressService progressService)
        {
            ModalService = modalService;
            DataService = dataService;
            ProgressService = progressService;

            FolderList = new FolderListViewModel(ModalService, DataService, ProgressService);
            NoteList = new NoteListViewModel(ModalService, DataService, ProgressService, FolderList);

            ShowSettingsCommand = new RelayCommand(ShowSettings);

            FolderList.SelectionChanged += async (s, e) =>
            {
                if (e.Entity != null)
                {
                    await NoteList.Load(e.Entity.Id);
                }
                else
                {
                    NoteList.Clear();
                }
            };

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            Load();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private async Task Load()
        {
            await FolderList.Load();
        }

        private async Task Clear()
        {
            await ProgressService.RunAsync(async () =>
            {
                await DataService.Notes.DeleteAll();
                await DataService.Folders.DeleteAll();
            });
        }

#if DEBUG
        public async Task ResetData()
        {
            await Clear();
            await Load();
        }
#endif

        private void ShowSettings()
        {
            ModalService.Show(new SettingsViewModel(DataService));
        }

        private readonly Guid tileGuid = new Guid("7610751c-f243-4a33-b5b6-7d7934152f47");

        public async Task Band()
        {
            try
            {
                var pairedBands = await BandClientManager.Instance.GetBandsAsync();
                using (var bandClient = await BandClientManager.Instance.ConnectAsync(pairedBands[0]))
                {
                    await RemoveBandTiles(bandClient);

                    // Create the small and tile icons from writable bitmaps.
                    // Small icons are 24x24 pixels.
                    var smallIconBitmap = new WriteableBitmap(24, 24);
                    var smallIcon = smallIconBitmap.ToBandIcon();

                    // Tile icons are 46x46 pixels for Microsoft Band 1, and 48x48 pixels
                    // for Microsoft Band 2.
                    var tileIconBitmap = new WriteableBitmap(46, 46);
                    var tileIcon = tileIconBitmap.ToBandIcon();

                    // create a new Guid for the tile

                    // create a new tile with a new Guid
                    var tile = new BandTile(tileGuid)
                    {
                        // enable badging (the count of unread messages)
                        IsBadgingEnabled = true,

                        // set the name
                        Name = "OneDo",

                        // set the icons
                        SmallIcon = smallIcon,
                        TileIcon = tileIcon
                    };

                    var designed = new BandTileLayout();
                    tile.PageLayouts.Add(designed.Layout);
                    await designed.LoadIconsAsync(tile);

                    if (await bandClient.TileManager.GetRemainingTileCapacityAsync() > 0)
                    {
                        // add the tile to the Band
                        if (await bandClient.TileManager.AddTileAsync(tile))
                        {
                            await bandClient.TileManager.SetPagesAsync(tileGuid, new PageData(Guid.NewGuid(), 0, designed.Data.All));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // handle a Band connection exception
                Logger.Current.Error("Band", e);
            }
        }

        private async Task RemoveBandTiles(IBandClient bandClient)
        {
            foreach (var tile in await bandClient.TileManager.GetTilesAsync())
            {
                // remove the tile from the Band
                if (await bandClient.TileManager.RemoveTileAsync(tile))
                {
                    // do work if the tile was successfully removed
                }
            }
        }
    }
}
