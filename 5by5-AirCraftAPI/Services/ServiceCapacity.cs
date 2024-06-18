using _5by5_AirCraftAPI.Models;

namespace _5by5_AirCraftAPI.Services
{
    // Daiane
    public class ServiceCapacity
    {
        public bool verifyCapacity(AirCraft airCraft)
        {
            if (airCraft.Capacity <= 0)
            {
                throw new ArgumentException("Capacidade invalida");
            }
           return true;
        }   
    }
}
