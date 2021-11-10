using Exemple.Domain.Models;
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
        public static ICaruciorProduse ValideazaProduse(Func<CodProdus, bool> checkProdusExists, CaruciorProduseNevalidate produseNevalidate)
        {
            List<ProdusValidat> produseValidate = new();
            bool isValidList = true;
            string invalidReson = string.Empty;
            foreach(var produsNevalidat in produseNevalidate.ListaProduse)
            {
                if (!CodProdus.TryParse(produsNevalidat.CodProdus, out CodProdus codProdus)
                    && checkProdusExists(codProdus))
                {
                    invalidReson = $"Cod produs invalid ({produsNevalidat.CodProdus})";
                    isValidList = false;
                    break;
                }
                if (!Produs.TryParseProdus(produsNevalidat.Cantitate, out Produs produs))
                {
                    invalidReson = $"Cantitatea invalida ({produsNevalidat.CodProdus}, {produsNevalidat.Cantitate})";
                    isValidList = false;
                    break;
                }
                if (!AdresaLivrare.TryParse(produsNevalidat.AdresaLivrare, out AdresaLivrare adresa))
                {
                    invalidReson = $"Adresa de livrare invalida ({produsNevalidat.CodProdus} , {produsNevalidat.AdresaLivrare})";
                    isValidList = false;
                    break;
                }
                if (!PretProdus.TryParse(produsNevalidat.Pret, out PretProdus pret))
                {
                    invalidReson = $"Pret invalid ({produsNevalidat.CodProdus}, {produsNevalidat.Pret})";
                    isValidList = false;
                    break;
                }
                    ProdusValidat produsValid = new(produs, pret, codProdus, adresa);
                    produseValidate.Add(produsValid);
            }

            if (isValidList)
            {
                return new CaruciorProduseValidate(produseValidate);
            }
            else
            {
                return new CaruciorProduseGol();
            }

        }

        public static ICaruciorProduse CalculeazaCaruciorProduse (ICaruciorProduse produse) => produse.Match(
            whenCaruciorProduseGol: caruciorGol => caruciorGol,
            whenCaruciorProduseNevalidate: caruciorProduseNevalidate => caruciorProduseNevalidate,
            whenCaruciorProduseCalculate: caruciorProduseCalculate => caruciorProduseCalculate,
            whenCaruciorProdusePlatite: caruciorPlatit => caruciorPlatit,
            whenCaruciorProduseValidate: produseValide =>
            {
                var produseCalculate = produseValide.ListaProduse.Select(produsValid =>
                                            new ProdusCalculat(produsValid.cantitate,
                                                                      produsValid.pret,
                                                                      produsValid.codProdus,
                                                                      produsValid.adresaLivrare,
                                                                      produsValid.pret * produsValid.cantitate.Cantitate
                                                                      ));
                return new CaruciorProduseCalculate(produseCalculate.ToList().AsReadOnly());
            }
        );

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
    }
}
