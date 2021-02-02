using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    abstract class OrganizationalUnitInterface
    {
        public OrganizationalUnitInterface() { }

        public abstract void PrintStructure();

        public abstract void PrintState();

        public abstract void PrintRentals(Activity activity);

        public abstract void PrintEarnings(Activity activity);

        public abstract void PrintBills(Activity activity);

        public virtual void Add(OrganizationalUnitInterface component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(OrganizationalUnitInterface component)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsUnit()
        {
            return true;
        }
    }
}
