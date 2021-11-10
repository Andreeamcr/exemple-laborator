using Exemple.Domain.Models;
using static LanguageExt.Prelude;
using LanguageExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Exemple.Domain.Models.CaruciorProduse;

namespace Exemple.Domain
{
    public static class CaruciorOperations
    {
        public static Task<ICaruciorProduse> ValideazaProduse(Func<CodProdus, TryAsync<bool>> checkProdusExists, CaruciorProduseNevalidate produseNevalidate) =>
            produseNevalidate.ListaProduse
                             .Select(ValideazaProdus(checkProdusExists))
                             .Aggregate(CreateEmptyListaProduse().ToAsync(), ReduceValidProduse)
                             .MatchAsync(
                                Right: produseValidate => new CaruciorProduseValidate(produseValidate),
                                LeftAsync: errorMessage => Task.FromResult((ICaruciorProduse)new CaruciorProduseGol(errorMessage))
                    );


         private static Func<ProdusNevalidat, EitherAsync<string, ProdusValidat>> ValideazaProdus(Func<CodProdus, TryAsync<bool>> checkProdusExists) =>
            produsNevalidat => ValideazaProdus(checkProdusExists, produsNevalidat);

        private static EitherAsync<string, ProdusValidat> ValideazaProdus(Func<CodProdus, TryAsync<bool>> checkProdusExists, ProdusNevalidat produsNevalidat) =>
            from produs in Produs.TryParseProdus(produsNevalidat.Cantitate)
                                 .ToEitherAsync(() => $"Cantitate invalida ({produsNevalidat.CodProdus}, {produsNevalidat.Cantitate})")
            from pretProdus in PretProdus.TryParse(produsNevalidat.Pret)
                                .ToEitherAsync(() => $"Pret invalid ({produsNevalidat.CodProdus}, {produsNevalidat.Pret})")
            from codProdus in CodProdus.TryParse(produsNevalidat.CodProdus)
                                .ToEitherAsync(() => $"Cod produs invalid ({produsNevalidat.CodProdus})")
            from adresaLivrare in AdresaLivrare.TryParse(produsNevalidat.AdresaLivrare)
                                .ToEitherAsync(() => $"Adresa introdusa este invalida ({produsNevalidat.AdresaLivrare})")
            from produsExists in checkProdusExists(codProdus)
                                .ToEither(error => error.ToString())
            select new ProdusValidat(produs, pretProdus, codProdus, adresaLivrare);


        private static Either<string, List<ProdusValidat>> CreateEmptyListaProduse() =>
           Right(new List<ProdusValidat>());

        private static EitherAsync<string, List<ProdusValidat>> ReduceValidProduse(EitherAsync<string, List<ProdusValidat>> acc, EitherAsync<string, ProdusValidat> next) =>
         from list in acc
         from nextProdus in next
         select list.AppendProdusValidat(nextProdus);

        private static List<ProdusValidat> AppendProdusValidat(this List<ProdusValidat> list, ProdusValidat produsValid)
        {
            list.Add(produsValid);
            return list;
        }

        public static ICaruciorProduse CalculeazaCaruciorProduse (ICaruciorProduse produse) => produse.Match(
            whenCaruciorProduseGol: caruciorGol => caruciorGol,
            whenCaruciorProduseNevalidate: caruciorProduseNevalidate => caruciorProduseNevalidate,
            whenCaruciorProduseCalculate: caruciorProduseCalculate => caruciorProduseCalculate,
            whenCaruciorProdusePlatite: caruciorPlatit => caruciorPlatit,
            whenCaruciorProduseValidate: CreazaCaruciorProduseCalculate
        );

        private static ICaruciorProduse CreazaCaruciorProduseCalculate(CaruciorProduseValidate produseValide) =>
            new CaruciorProduseCalculate(produseValide.ListaProduse
                                                      .Select(CalculeazaPretTotal)
                                                      .ToList()
                                                      .AsReadOnly());
        
        private static ProdusCalculat CalculeazaPretTotal(ProdusValidat produsValidat) =>
            new ProdusCalculat(produsValidat.cantitate,
                              produsValidat.pret,
                              produsValidat.codProdus,
                              produsValidat.adresaLivrare,
                              produsValidat.pret * produsValidat.cantitate.Cantitate);

        public static ICaruciorProduse PlatesteCaruciorProduse(ICaruciorProduse produse) => produse.Match(
             whenCaruciorProduseGol: caruciorGol => caruciorGol,
            whenCaruciorProduseNevalidate: caruciorProduseNevalidate => caruciorProduseNevalidate,
            whenCaruciorProduseValidate: produseValide => produseValide,
            whenCaruciorProdusePlatite: caruciorPlatit => caruciorPlatit,
            whenCaruciorProduseCalculate: caruciorProduseCalculate => 
            {
                StringBuilder csv = new();
                caruciorProduseCalculate.ListaProduse.Aggregate(csv, (export, produs) => export.AppendLine($"{produs.codProdus.Value}, {produs.cantitate}, {produs.adresaLivrare},  {produs.pretTotal}"));

                CaruciorProdusePlatite caruciorPlatit = new(caruciorProduseCalculate.ListaProduse, csv.ToString(), DateTime.Now);

                return caruciorPlatit;
            });

            private static ICaruciorProduse GenerateExport(CaruciorProduseCalculate produseCalculate) =>
            new CaruciorProdusePlatite(produseCalculate.ListaProduse,
                                       produseCalculate.ListaProduse.Aggregate(new StringBuilder(), CreateCsvLine).ToString(),
                                       DateTime.Now);

            private static StringBuilder CreateCsvLine(StringBuilder export, ProdusCalculat produs) =>
                export.AppendLine($"{produs.codProdus.Value}, {produs.cantitate}, {produs.pret}, {produs.adresaLivrare}, {produs.pretTotal}");
    }
}
