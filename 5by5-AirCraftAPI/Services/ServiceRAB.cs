using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Text.RegularExpressions;

namespace _5by5_AirCraftAPI.Services
{
    public class ServiceRAB
    {
        public string ValidateRAB(string rab)
        {
            var RAB = rab.ToUpper(); 

            string pattern = @"^PP-[A-Z]{3}$";

            if (!Regex.IsMatch(RAB, pattern))
            {
                throw new ArgumentException("Formato RAB inválido. Deve seguir o padrão 'PP-ABC'.");
            }

            return RAB;
        }

        

    }
}
