using System.ComponentModel.DataAnnotations;

namespace _5by5_AirCraftAPI.Models
{
    public class AirCraftDTO
    {
        public string Rab { get; set; }
        public int Capacity { get; set; }
        public string DTRegistry { get; set; }
        public string DTLastFlight { get; set; }
        public string CnpjCompany { get; set; }
    }
}
