using _5by5_AirCraftAPI.Models;
using Microsoft.EntityFrameworkCore;
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
            if (response.IsSuccessStatusCode)
            {
                var company = JsonConvert.DeserializeObject<Company>(response.Content.ReadAsStringAsync().Result);
                if (company != null && company.Cnpj.Length >= 14)
                {
                    
                    return company.Cnpj ;
                }
                
            }
            return "";
        }
        public string CnpjMask(string cnpj)
        {
            var mask = $"{cnpj.Substring(0, 2)}.{cnpj.Substring(2, 3)}.{cnpj.Substring(5, 3)}/{cnpj.Substring(8, 4)}-{cnpj.Substring(12)}";
            return mask;
        }
    }
}
