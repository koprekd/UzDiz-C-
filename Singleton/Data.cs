using dkoprek_zadaca_3;
using System;
using System.Collections.Generic;
using System.Text;

namespace dkoprek_zadaca_3
{
    class Database
    {
        private string[] args;
        public List<Activity> Activities = new List<Activity>();
        public List<Location> Locations = new List<Location>();
        public List<LocationCapacity> LocationCapacities = new List<LocationCapacity>();
        public List<Person> Persons = new List<Person>();
        public List<PriceList> PriceLists = new List<PriceList>();
        public List<VehicleType> VehicleTypes = new List<VehicleType>();
        public List<Vehicle> Vehicles = new List<Vehicle>();
        public List<Rental> ActiveRentals = new List<Rental>();
        public List<Rental> CompletedRentals = new List<Rental>();
        public List<Bill> Bills = new List<Bill>();
        public int BillCount = 0;
        public double MaxAllowedDebt;
        public OrganizationalUnit OrganizationTree = new OrganizationalUnit();

        private List<OrganizationUnitObject> OrganizationUnitObjects = new List<OrganizationUnitObject>();
        public List<OrganizationalUnit> OrganizationalUnits = new List<OrganizationalUnit>();
        private Database() { }

        private static Database _instance;

        private static readonly object _lock = new object();

        public static Database GetInstance()
        {

            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new Database();
                    }
                }
            }
            return _instance;
        }

        public void LoadData(string[] args)
        {
            this.SetArgs(args);
            List<Object> PersonsAsObjects = this.GetListOfObjects(new PersonLoaderFactory());
            List<Object> VehiclesAsObjects = this.GetListOfObjects(new VehicleTypeLoaderFactory());
            List<Object> ActivitiesAsObjects = this.GetListOfObjects(new ActivityLoaderFactory());
            List<Object> LocationsAsObjects = this.GetListOfObjects(new LocationLoaderFactory());
            List<Object> LocationCapacitiesAsObjects = this.GetListOfObjects(new LocationCapacityLoaderFactory());
            List<Object> PriceListsAsObjects = this.GetListOfObjects(new PriceListLoaderFactory());
            List<Object> OrganizationUnitsAsObjects = this.GetListOfObjects(new OrganizationUnitObjectLoaderFactory());

            foreach (Object item in PersonsAsObjects)
            {
                this.Persons.Add(item as Person);
            }

            foreach (Object item in VehiclesAsObjects)
            {
                this.VehicleTypes.Add(item as VehicleType);
            }

            foreach (Object item in ActivitiesAsObjects)
            {
                this.Activities.Add(item as Activity);
            }

            foreach (Object item in LocationsAsObjects)
            {
                this.Locations.Add(item as Location);
            }

            foreach (Object item in LocationCapacitiesAsObjects)
            {
                this.LocationCapacities.Add(item as LocationCapacity);
            }

            foreach (Object item in PriceListsAsObjects)
            {
                this.PriceLists.Add(item as PriceList);
            }

            foreach (Object item in OrganizationUnitsAsObjects)
            {
                this.OrganizationUnitObjects.Add(item as OrganizationUnitObject);
            }

            this.LoadVehicles();
            this.FillOrganizationUnitList();
            this.FillOrganizationTree();

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-dug")
                {
                    string aux;
                    aux = args[i + 1].Replace(',','.');
                    this.MaxAllowedDebt = Convert.ToDouble(aux);
                }
            }
          
        }

        private void SetArgs(string[] args)
        {
            this.args = args;
        }

        private List<Object> GetListOfObjects (LoaderFactory loader)
        {
            return loader.GetObjects(args);
        }

        private void LoadVehicles()
        {
            int vehicleCounter = 0;
            foreach (LocationCapacity item in this.LocationCapacities)
            {
                for (int i = 0; i < item.AvailableVehiclesNumber; i++)
                {
                    Vehicle newVehicle = new Vehicle(new VehicleStateFree());
                    newVehicle.Id = vehicleCounter;
                    vehicleCounter++;
                    newVehicle.VehicleTypeId = item.VehicleTypeId;
                    newVehicle.LastLocation = item.LocationId;
                    newVehicle.Charge = 100;
                    newVehicle.TotalKms = 0;
                    newVehicle.Rented = false;
                    newVehicle.Broken = false;
                    newVehicle.NumberOfRentals = 0;
                    this.Vehicles.Add(newVehicle);
                }
            }
        }

        private void FillOrganizationTree()
        {
            this.OrganizationTree = this.OrganizationalUnits[0];

            foreach (OrganizationalUnit item in this.OrganizationalUnits)
            {
                if (item.ParentId != -1) OrganizationalUnits.Find(u => u.Id == item.ParentId).Add(item);
                foreach (int locationID in item.LocationIds) item.Add(Database.GetInstance().Locations.Find(l => l.Id == locationID));
            }
        }

        private void FillOrganizationUnitList() // punjenje pomocne liste
        {
            foreach (OrganizationUnitObject item in this.OrganizationUnitObjects)
            {
                OrganizationalUnit unit = new OrganizationalUnit();
                unit.Id = item.Id;
                unit.Name = item.Name;
                unit.ParentId = item.SuperiorUnit;
                unit.LocationIds = item.LocationIds;
                this.OrganizationalUnits.Add(unit);
            }
        }
    }
}
