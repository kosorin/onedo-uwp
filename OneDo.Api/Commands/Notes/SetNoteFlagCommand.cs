using OneDo.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Application.Commands.Notes
{
    public class SetNoteFlagCommand : ICommand
    {
        public SetNoteFlagCommand(Guid id, bool isFlagged)
        {
            Id = id;
            IsFlagged = isFlagged;
        }

        public Guid Id { get; set; }

        public bool IsFlagged { get; set; }
    }
}