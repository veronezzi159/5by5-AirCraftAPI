using _5by5_AirCraftAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
            try
            {
                var response = _client.GetAsync($"https://localhost:7269/api/Companies/{cnpj}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var company = JsonConvert.DeserializeObject<Company>(response.Content.ReadAsStringAsync().Result);
                    if (company != null && company.Cnpj.Length >= 14)
                    {
                        return company.Cnpj;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
               throw new ArgumentException ("Erro ao enviar requisições para a API " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("API não encontrada " + ex.Message);
            }
            return "";
        }
    }
}
