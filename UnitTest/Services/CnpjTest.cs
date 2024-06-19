using _5by5_AirCraftAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.Services
{
    public class CnpjTest
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        public async Task<AirCraft> PostAirCraft(AirCraft aircraft)
        {
            try
            {
                var content = new StringContent(JsonConvert.SerializeObject(aircraft), Encoding.UTF8, "application/json");
                HttpResponseMessage response = await _httpClient.PostAsync("https://localhost:7006/api/AirCraft", content);
                response.EnsureSuccessStatusCode();
                return JsonConvert.DeserializeObject<AirCraft>(await response.Content.ReadAsStringAsync()); 
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
