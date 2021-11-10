using Exemple.Domain.Models;
using System;
using System.Collections.Generic;
using static Exemple.Domain.Models.CaruciorProduse;
using Exemple.Domain;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using System.Net.Http;

namespace Exemple
{
    class Program
    {
        private static readonly Random random = new Random();

        static async Task Main(string[] args)
        {
            var listaProduse = ReadListaProduse().ToArray();
            PlataCaruciorCommand command = new(listaProduse);
            PlataCaruciorWorkflow workflow = new();
            var result = await workflow.ExecuteAsync(command, checkProdusExists);

            result.Match(
                    whenCaruciorProdusePlatiteFailedEvent: @event =>
                    {
                        Console.WriteLine($"Plata eronata: {@event.Reason}");
                        return @event;
                    },
                    whenCaruciorProdusePlatiteSucceededEvent: @event =>
                    {
                        Console.WriteLine($"Plata facuta cu success.");
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
                );
        }

        private static int SumaTotala()
        {
            Option<int> two = Some(2);
            Option<int> four = Some(4);
            Option<int> six = Some(6);
            Option<int> none = None;

            var result = from x in two
                         from y in four
                         from z in six
                         from n in none
                         select x + y + z + n;
            // This expression succeeds because all items to the right of 'in' are Some of int
            // and therefore it lands in the Some lambda.
            int r = match(result,
                           Some: v => v * 2,
                           None: () => 0);     // r == 24

            return r;
        }

        private static List<ProdusNevalidat> ReadListaProduse()
        {
            List<ProdusNevalidat> listaProduse = new();
            do
            {
                //read registration number and grade and create a list of greads
                var cantitate = ReadValue("Cantitate: ");
                if (string.IsNullOrEmpty(cantitate))
                {
                    break;
                }
                var pret = ReadValue("Pret: ");
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

                listaProduse.Add(new(cantitate, pret, codProdus, adresaLivrare));
            } while (true);
            return listaProduse;
        }

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        private static TryAsync<bool> checkProdusExists(CodProdus student)
        {
            Func<Task<bool>> func = async () =>
            {
                //HttpClient client = new HttpClient();

                //var response = await client.PostAsync($"www.university.com/checkRegistrationNumber?number={student.Value}", new StringContent(""));

                //response.EnsureSuccessStatusCode(); //200

                return true;
            };
            return TryAsync(func);
        }
    }
}
