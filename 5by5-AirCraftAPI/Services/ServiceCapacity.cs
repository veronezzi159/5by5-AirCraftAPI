using _5by5_AirCraftAPI.Models;
using System;

namespace _5by5_AirCraftAPI.Services
{
    
    public class ServiceCapacity
    {
        /// <summary>
        /// Verifica se a capacidade da aeronave é válida.
        /// A capacidade deve ser maior que 0 e não deve exceder um limite máximo definido.
        /// </summary>
        /// 
        ///<param name="airCraft">A aeronave a ser verificada.</param>
        ///<returns> True se a capacidade for valida, em não sendo trás uma execeção</returns>
        ///<excetion cref="ArgumentException">la'ncada quand a capacidade for invalida .</excetion>
        
        private const int maxCapacity = 240;
        public bool verifyCapacity(AirCraft airCraft)
        {
            if (airCraft.Capacity <= 0)
            {
                throw new ArgumentException("Capacidade invalida: Capacidade deve ser sempre maior que o.");
            }
            else if (airCraft.Capacity > maxCapacity)
            {
                throw new ArgumentException ("Capacidade invalida: Capacidade excedeu o limite maximo e deve ser menor ou igual a 240.");
            }
           return true;
        }   
    }
}


/// <summary>
/// Daiane
/// </summary>
// Aqui ele está verificando se a capacidade do avião é válida, usa o método verifyCapacity para verificar se a capacidade é válida, se não for válida ele lança uma exceção com a mensagem "Capacidade invalida" 