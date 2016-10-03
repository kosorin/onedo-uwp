﻿using GalaSoft.MvvmLight;
using OneDo.Model.Data;
using OneDo.Services.ModalService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel
{
    public abstract class ModalViewModel : ExtendedViewModel
    {
        public IModalService ModalService { get; }

        public DataService DataService { get; }

        public event EventHandler Closed;

        protected ModalViewModel(IModalService modalService, DataService dataService)
        {
            ModalService = modalService;
            DataService = dataService;
        }

        public void Close()
        {
            ModalService.Close();
            Closed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnClosed()
        {

        }
    }
}
