using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiComunidadPro.Common.Settings
{
    public class CacheSettings
    {
        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }
        public List<ExpirationTime> ExpirationTimes { get; set; } = new List<ExpirationTime>();
    }

    public class ExpirationTime
    {
        public string Key { get; set; }
        public int Duration { get; set; }
    }
}
