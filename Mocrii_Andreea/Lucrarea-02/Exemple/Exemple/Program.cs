using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using static Exemple.Domain.Models.CaruciorProduse;
using static Exemple.Domain.CaruciorOperations;
using Exemple.Domain;

namespace Exemple
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var listaProduse = ReadListaProduse().ToArray();
            PlataCaruciorCommand command = new(listaProduse);
            PlataCaruciorWorkflow workflow = new PlataCaruciorWorkflow();
            var result = workflow.Execute(command, (CodProdus) => true);

            result.Match(
                    whenCarucriorProdusePlatiteFaildEvent: @event =>
                    {
                        Console.WriteLine($"Publish failed: {@event.Reason}");
                        return @event;
                    },
                    whenCarucriorProdusePlatiteScucceededEvent: @event =>
                    {
                        Console.WriteLine($"Publish succeeded.");
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
                );

            Console.WriteLine("Hello World!");
        }

        private static List<ProdusNevalidat> ReadListaProduse()
        {
            List <ProdusNevalidat> listaProduse = new();
            do
            {
                //read registration number and grade and create a list of greads
                var cantitate = ReadValue("Cantitate: ");
                if (string.IsNullOrEmpty(cantitate))
                {
                    break;
                }

                var pret = ReadValue("Pret produs: ");
                if (string.IsNullOrEmpty(pret))
                {
                    break;
                }

                var codProdus = ReadValue("Cod produs: ");
                if (string.IsNullOrEmpty(codProdus))
                {
                    break;
                }

                var adresaLivrare = ReadValue("Adresa de livrare: ");
                if (string.IsNullOrEmpty(adresaLivrare))
                {
                    break;
                }

                listaProduse.Add(new (cantitate, pret, codProdus, adresaLivrare));
            } while (true);
            return listaProduse;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
