using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    class PriceListLoaderFactory : LoaderFactory
    {
        public override Loader CreateLoader()
        {
            return new PriceListLoader();
        }
    }
}
