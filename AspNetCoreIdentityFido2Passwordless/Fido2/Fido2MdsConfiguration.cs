﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentityFido2Passwordless
{
    public class Fido2MdsConfiguration
    {
        public string MDSAccessKey { get; set; }
        public string MDSCacheDirPath { get; set; }
    }
}
