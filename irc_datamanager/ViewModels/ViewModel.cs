﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace irc_datamanager.ViewModels
{
    public abstract class ViewModel
    {
        public virtual string ViewModelName
        {
            get
            {
                return "ViewModelName";
            }
        }
    }
}
