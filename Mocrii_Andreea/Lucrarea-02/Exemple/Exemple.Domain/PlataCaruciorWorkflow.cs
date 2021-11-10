using Exemple.Domain.Models;
using static Exemple.Domain.Models.CarucriorProdusePlatiteEvent;
using static Exemple.Domain.CaruciorOperations;
using System;
using static Exemple.Domain.Models.CaruciorProduse;

namespace Exemple.Domain
{
    public class PlataCaruciorWorkflow
    {
        public ICarucriorProdusePlatiteEvent Execute(PlataCaruciorCommand command, Func<CodProdus, bool> checkProdusExists)
        {
            CaruciorProduseNevalidate produseNevalidate = new CaruciorProduseNevalidate(command.InputProduse);
            ICaruciorProduse produse = ValideazaProduse(checkProdusExists, produseNevalidate);
            produse = CalculeazaCaruciorProduse(produse);
            produse = PlatesteCaruciorProduse(produse);

            return produse.Match(
                    whenCaruciorProduseGol: carucioGol => new CarucriorProdusePlatiteFaildEvent("Unexpected unvalidated state") as ICarucriorProdusePlatiteEvent,
                    whenCaruciorProduseNevalidate: caruciorProduseNevalidate => new CarucriorProdusePlatiteFaildEvent("Nu s-au putut valida"),
                    whenCaruciorProduseValidate: produseValidate => new CarucriorProdusePlatiteFaildEvent("Unexpected validated state"),
                    whenCaruciorProduseCalculate: produseCalculate => new CarucriorProdusePlatiteFaildEvent("Unexpected calculated state"),
                    whenCaruciorProdusePlatite: produsePlatite => new CarucriorProdusePlatiteScucceededEvent(produsePlatite.Csv, produsePlatite.PublishedDate)
                );
        }
    }
}
