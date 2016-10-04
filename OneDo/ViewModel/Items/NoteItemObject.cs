﻿using GalaSoft.MvvmLight;
using OneDo.Model.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.ViewModel.Items
{
    public class NoteItemObject : ItemObject<Note>
    {
        public bool IsCompleted => Entity.Completed != null;

        public bool IsFlagged => Entity.IsFlagged;

        public string Title => Entity.Title;

        public string Text => Entity.Text;

        public bool HasText => !string.IsNullOrWhiteSpace(Text);

        public DateTime? Date => Entity.Date;

        public bool HasDate => Date != null;

        public INoteCommands Commands { get; }

        public NoteItemObject(Note entity, INoteCommands commands) : base(entity)
        {
            Commands = commands;
        }
    }
}
