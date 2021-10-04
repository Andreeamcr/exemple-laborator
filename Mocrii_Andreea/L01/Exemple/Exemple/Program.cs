using Exemple.Domain;
using System;
using System.Collections.Generic;
using static Exemple.Domain.ShoppingCart;

namespace Exemple
{
    class Program
    {
        private static readonly Random random = new Random();

        static void Main(string[] args)
        {
            var Cart = ReadListOfProducts.ToArray();
            UnvalidatedCart unvalidatedCart = new(listOfProducts);
            IShoppingCart result = ValidateCart(unvalidatedCart);
            result.Match(
                whenInvalidatedCart: unvalidatedResult => unvalidatedResult,
                whenPayedCart: publishedResult => publishedResult,
                whenInvalidatedCart: invalidResult => invalidResult,
                whenValidatedCartProducts: validatedResult => PayedCart(validatedResult)
            );

            Console.WriteLine("Hello World!");
        }

        private static List<UnvalidatedCart> ReadListOfProducts
        {
            get
            {
                List<UnvalidatedCart> listOfProducts = new();
                do
                {
                    //read registration number and grade and create a list of greads
                    var registrationNumber = ReadValue("Registration Number: ");
                    if (string.IsNullOrEmpty(registrationNumber))
                    {
                        break;
                    }

                    var quantity = ReadValue("Quantity: ");
                    if (string.IsNullOrEmpty(quantity))
                    {
                        break;
                    }

                    listOfProducts.Add(new(registrationNumber, quantity));
                } while (true);
                return listOfProducts;
            }
        }

        private static IShoppingCart ValidateCart(UnvalidatedCart unvalidatedCart) =>
            random.Next(100) > 50 ?
            new InvalidatedCart(new List<UnvalidatedCart>(), "Random errror")
            : new ValidatedCartProducts(new List<ValidatedCart>());

        private static IShoppingCart PayedCart(ValidatedCart validCart) =>
            new PayedCart(new List<ValidatedCart>(), DateTime.Now);

        private static string? ReadValue(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
    }
}
