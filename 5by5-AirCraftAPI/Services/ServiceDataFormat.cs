namespace _5by5_AirCraftAPI.Services
{
    public class ServiceDataFormat
    {
        public string MaskDate(DateTime date)
        {
           
            return $"{date.ToString().Substring(0,10)}";
        }
    }
}
