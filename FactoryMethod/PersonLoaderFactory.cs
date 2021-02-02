using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    class PersonLoaderFactory : LoaderFactory
    {
        public override Loader CreateLoader()
        {
            return new PersonLoader();
        }
    }
}
