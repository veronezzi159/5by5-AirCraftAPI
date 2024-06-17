using System.ComponentModel.DataAnnotations;

namespace _5by5_AirCraftAPI.Models
{
    public class Removed
    {

        public int Id { get; set; }
        [StringLength(6)]
        public string Rab { get; set; }
        public int Capacity { get; set; }
        public DateTime DTRegistry { get; set; }
        public DateTime DTLastFlight { get; set; }
        [StringLength(19)]
        public string CnpjCompany { get; set; }
    }
}
