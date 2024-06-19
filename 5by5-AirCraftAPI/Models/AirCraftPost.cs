using System.ComponentModel.DataAnnotations;

namespace _5by5_AirCraftAPI.Models
{
    public class AirCraftPost
    {
        [StringLength(6)]
        public string Rab { get; set; }
        public int Capacity { get; set; }
        [StringLength(19)]
        public string CnpjCompany { get; set; }
    }
}
