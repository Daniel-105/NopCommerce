﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Payments.Klarna.Interface
{
    public interface IAsyncInitialization
    {       
        Task Initialization { get; }
    }
}
