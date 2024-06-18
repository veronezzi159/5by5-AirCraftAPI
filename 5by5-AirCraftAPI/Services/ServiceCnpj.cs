using _5by5_AirCraftAPI.Models;
using Newtonsoft.Json;

namespace _5by5_AirCraftAPI.Services
{
    public class ServiceCnpj
    {
        private readonly HttpClient _client = new HttpClient();
       
        public string ValidationCnpj(string cnpj)
        {
            //consume the company api
            var response = _client.GetAsync($"https://localhost:7086/api/CompanyAPI/{cnpj}").Result;
            var Company = JsonConvert.DeserializeObject<Company>(response.Content.ReadAsStringAsync().Result);
            if (Company == null)
            {
                return "";
            }
            else { return Company.Cnpj;}
        }
        
    }
}
