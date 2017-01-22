using GalaSoft.MvvmLight.Messaging;
using OneDo.Application.Common;
using OneDo.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Messages
{
    public class ShowEntityEditorMessage<TEntity> : IMessage
        where TEntity : IEntityModel
    {
        public ShowEntityEditorMessage(TEntity entity)
        {
            Entity = entity;
        }

        public TEntity Entity { get; }
    }
}