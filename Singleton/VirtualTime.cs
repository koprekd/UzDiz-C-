using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    public class VirtualTime
    {

        public DateTime CurrentTime;

        private VirtualTime() { }

        private static VirtualTime _instance;

        private static readonly object _lock = new object();

        public static VirtualTime GetInstance()
        {

            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new VirtualTime();
                    }
                }
            }
            return _instance;
        }
    }
}
