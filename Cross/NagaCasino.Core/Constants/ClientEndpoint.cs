using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechDrum.Core.Constants
{
    public class ClientEndpoint
    {
        public const string ClientAreas = "home";
        public static class Home
        {
            private const string BaseEndpoint = "~/" + ClientAreas;
            public const string IndexEndpoint = BaseEndpoint;
        }
    }
}
