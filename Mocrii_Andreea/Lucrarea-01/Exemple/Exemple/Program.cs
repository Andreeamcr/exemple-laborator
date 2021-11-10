using Exemple.Domain;
using System;
using System.Collections.Generic;
using static Exemple.Domain.Carucior;
using System.Linq;

namespace Exemple
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {            
            var listaProduse = CitesteLista();
            CaruciorGol caruciorGol = new CaruciorGol();
            ICarucior result = AdaugaProduse(listaProduse);
            result.Match(
                whenCaruciorGol: caruciorGol => caruciorGol,
                whenCaruciorNevalidat: caruciorNevalidat => ValideazaProduse(caruciorNevalidat),
                whenCaruciorValidat: caruciorValidat => PlatesteProduse(caruciorValidat),
                whenCaruciorPlatit: caruciorPlatit => caruciorPlatit
                );

            Console.WriteLine("Hello World!");
        }

        private static List<ProdusNevalidat> CitesteLista()
        {
            List<ProdusNevalidat> listaProduse = new();
            do
            {
                //citim produsul
                var cantitate = ReadValue("Cantitate: ");
                if (string.IsNullOrEmpty(cantitate))
                {
                    break;
                }
                var cod = ReadValue("Cod produs: ");
                if (string.IsNullOrEmpty(cod))
                {
                    break;
                }
                var adresa = ReadValue("Adresa: ");
                if (string.IsNullOrEmpty(adresa))
                {
                    break;
                }

                listaProduse.Add(new(cantitate, cod, adresa));
            } while (true);
            return listaProduse;
        }

        private static ICarucior AdaugaProduse(List<ProdusNevalidat> produseNevalidate) => produseNevalidate.Count == 0 ?
            new CaruciorGol() : new CaruciorNevalidat(produseNevalidate);

        private static ICarucior ValideazaProduse(CaruciorNevalidat carucior)
        {
            List<Produs> produseValidate = new();
            foreach(var produs in carucior.ListaProduse)
            {
                produseValidate.Add(new Produs(produs.Cantitate, produs.CodProdus, produs.Adresa));
            }
            return new CaruciorValidat(produseValidate);
        }
        private static ICarucior PlatesteProduse(CaruciorValidat carucior) =>
             new CaruciorPlatit(carucior.ListaProduse, DateTime.Now);

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
