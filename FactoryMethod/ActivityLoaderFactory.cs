using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    class ActivityLoaderFactory : LoaderFactory
    {
        public override Loader CreateLoader()
        {
            return new ActivityLoader();
        }
    }
}
