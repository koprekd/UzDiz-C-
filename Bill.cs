using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dkoprek_zadaca_3
{
    public class Bill
    {
        public int Id { get; set; }
        public float RentalFee { get; set; }
        public float HourlyFee { get; set; }
        public float DistanceFee { get; set; }
        public float TotalAmount { get; set; }
        public DateTime IssueTime { get; set; }
        public int CustomerId { get; set; }
        public int RentalLocationId { get; set; }
        public int ReturnLocationId { get; set; }
        public bool PaidFor { get; set; }

        public void PrintBill()
        {
            Console.WriteLine($"~~~~~~~Račun {this.Id}~~~~~~~~~");
            Console.WriteLine($"Cijena najma: {RentalFee}");
            Console.WriteLine($"Cijena vremena najma: {HourlyFee}");
            Console.WriteLine($"Cijena prijeđenih kilometara: {DistanceFee}");
            Console.WriteLine($"UKUPNO:   {TotalAmount}");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~");
        }
    }
}
