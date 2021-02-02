using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    class OrganizationUnitObjectLoaderFactory : LoaderFactory
    {
        public override Loader CreateLoader()
        {
            return new OrganizationUnitObjectLoader();
        }
    }
}
