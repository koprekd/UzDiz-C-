using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    class LocationLoaderFactory : LoaderFactory
    {
        public override Loader CreateLoader()
        {
            return new LocationLoader();
        }
    }
}
