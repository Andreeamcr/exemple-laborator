using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public record Produs
    {
        private static readonly Regex ValidPattern = new("^[0-9]*$");

        public decimal Cantitate { get; }

        public string Cod { get; }

        public string Adresa { get; }

        public Produs(string cantitate, string cod, string adresa)
        {
            if (Convert.ToDecimal(cantitate) >= 0)
            {
                Cantitate = Convert.ToDecimal(cantitate);
            }
            else
            {
                throw new ProdusInvalidException($"{cantitate:0.##} este o cantitate invalida");
            }

            if(ValidPattern.IsMatch(cod))
            {
                Cod = cod;
            }
            else 
            {
                throw new ProdusInvalidException($"{cod:0.##} este un cod invalid");
            }

            Adresa = adresa;
        }


        public override string ToString()
        {
            return $"{Cantitate:0.##}";
        }
    }
}
