using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CoreIdentityWebApi.Models
{
    public class AppUser : IIdentity
    {
        //public string AuthenticationType()
        //{
        //    throw new NotImplementedException();
        //}

        public string AuthenticationType => throw new NotImplementedException();

        public bool IsAuthenticated => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();
    }
}
