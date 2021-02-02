using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    class DepthIterator : Iterator
    {
        private OrganizationalUnit root;
        public OrganizationalUnit currentOrgUnit;
        private List<OrganizationalUnit> visited = new List<OrganizationalUnit>();
        public DepthIterator(OrganizationalUnit organizationalUnit)
        {
            root = organizationalUnit;
            currentOrgUnit = organizationalUnit;
        }

        public OrganizationalUnit CurrentOrganizationalUnit()
        {
            return currentOrgUnit;
        }

        public bool HasNext()
        {
            foreach (OrganizationalUnit item in currentOrgUnit.SubUnits)
            {
                if (!visited.Contains(item)) return true;
            }
            return false;
        }

        public OrganizationalUnit NextOrganizationalUnit()
        {
            if (HasNext())
            {
                foreach (OrganizationalUnit item in currentOrgUnit.SubUnits)
                {
                    if (!visited.Contains(item))
                    {
                        return item;
                    }
                }
            }
            visited.Add(currentOrgUnit);
            if (root == currentOrgUnit) return root;
            foreach (OrganizationalUnit item in Database.GetInstance().OrganizationalUnits)
            {
                if (item.Id == currentOrgUnit.Id) currentOrgUnit = item;
            }
            return NextOrganizationalUnit();
        }
    }
}
