using LanguageExt;
using ShoppingApp.Domain;
using ShoppingApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LanguageExt;
using static LanguageExt.Prelude;
using System;
using Microsoft.Extensions.Logging;
using ShoppingApp.Data;
using Microsoft.EntityFrameworkCore;
using ShoppingApp.Data.Repositories;

namespace ShoppingApp
{
    class Program
    {
        private static readonly Random random = new Random();

        private static string ConnectionString = "Server=LAPTOP-5O6G7HEC\\DEVELOPER;Database=Shop-sample;Trusted_Connection=True;MultipleActiveResultSets=true";

        static async Task Main(string[] args)
        {
            using ILoggerFactory loggerFactory = ConfigureLoggerFactory();
            ILogger<CartWorkflow> logger = loggerFactory.CreateLogger<CartWorkflow>();

            var productList = ReadProductList().ToArray();
            CartCommand command = new(productList);

            var dbContextBuilder = new DbContextOptionsBuilder<ProductsContext>()
                                        .UseSqlServer(ConnectionString)
                                        .UseLoggerFactory(loggerFactory);
            
            ProductsContext productsContext = new ProductsContext(dbContextBuilder.Options);
            ProductsRepository productsRepository = new(productsContext);
            OrderHeadersRepository orderHeadersRepository = new(productsContext);
            OrderLinesRepository orderLinesRepository = new(productsContext);
            CartWorkflow workflow = new(productsRepository, orderLinesRepository, orderHeadersRepository, logger);
            var result = await workflow.ExecuteAsync(command);

            result.Match(
                    whenFailedPaidCartEvent: @event =>
                    {
                        Console.WriteLine($"Plata eronata: {@event.Reason}");
                        return @event;
                    },
                    whenSuccessPaidCartEvent: @event =>
                    {
                        Console.WriteLine($"Plata facuta cu success.");
                        Console.WriteLine(@event.Csv);
                        return @event;
                    }
                );
        }

        private static List<UnvalidatedProduct> ReadProductList()
        {
            List<UnvalidatedProduct> listaProduse = new();
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

        private static TryAsync<bool> checkProdusExists(ProductCode code)
        {
            Func<Task<bool>> func = async () =>
            {
                return true;
            };
            return TryAsync(func);
        }

        private static ILoggerFactory ConfigureLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
                                builder.AddSimpleConsole(options =>
                                {
                                    options.IncludeScopes = true;
                                    options.SingleLine = true;
                                    options.TimestampFormat = "hh:mm:ss ";
                                })
                                .AddProvider(new Microsoft.Extensions.Logging.Debug.DebugLoggerProvider()));
        }
    }
}
