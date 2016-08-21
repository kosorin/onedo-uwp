﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OneDo.Model.Data;
using OneDo.Model.Data.Entities;
using System.Text.RegularExpressions;

namespace OneDo.Model.Business
{
    public class FolderBusiness : EntityBusiness<Folder>
    {
        public FolderBusiness(ISettingsProvider settingsProvider) : base(settingsProvider)
        {

        }

        public override Folder Clone(Folder entity)
        {
            return entity;
        }

        public string NormalizeName(string name)
        {
            return (name ?? "").Trim();
        }
    }
}