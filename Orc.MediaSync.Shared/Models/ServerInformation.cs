﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Orc.MediaSync.Shared.Models
{
    public class ServerInformation
    {
        public Version MaximumSupportedClientVersion { get; set; }
        public Version ServerVersion { get; set; }
    }
}
