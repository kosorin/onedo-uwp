﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Common
{
    public interface IHandler<in TArgs>
    {
        void Handle(TArgs args);
    }
}