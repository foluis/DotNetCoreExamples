﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenAuthentcation.Models
{
    public class JwtConfig
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
    }
}
