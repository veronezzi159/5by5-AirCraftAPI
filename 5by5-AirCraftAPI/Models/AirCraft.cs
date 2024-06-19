using System.ComponentModel.DataAnnotations;

namespace _5by5_AirCraftAPI.Models
{
    public class AirCraft
    {
        [Key]
        [StringLength(6)]
        public string Rab { get; set; }
        public int Capacity { get; set; }        
        public DateTime DTRegistry { get; set; }        
        public DateTime DTLastFlight { get; set; }
        [StringLength(19)]
        public string CnpjCompany { get; set; }

        public AirCraft(AirCraftPost inserted)
        {
            this.Rab = inserted.Rab;
            this.Capacity = inserted.Capacity;
            this.CnpjCompany = inserted.CnpjCompany;
            this.DTRegistry = DateTime.Now;
            this.DTLastFlight = DateTime.Now;
        }
        public AirCraft()
        {
            
        }
    }
}
