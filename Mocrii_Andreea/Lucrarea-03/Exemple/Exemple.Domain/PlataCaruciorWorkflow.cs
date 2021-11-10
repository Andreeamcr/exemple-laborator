using Exemple.Domain.Models;
using static Exemple.Domain.Models.CaruciorProdusePlatiteEvent;
using static Exemple.Domain.CaruciorOperations;
using System;
using static Exemple.Domain.Models.CaruciorProduse;
using LanguageExt;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    public class PlataCaruciorWorkflow
    {
        public async Task<ICaruciorProdusePlatiteEvent> ExecuteAsync(PlataCaruciorCommand command, Func<CodProdus, TryAsync<bool>> checkProdusExists)
        {
            CaruciorProduseNevalidate produseNevalidate = new CaruciorProduseNevalidate(command.InputProduse);
            ICaruciorProduse produse = await ValideazaProduse(checkProdusExists, produseNevalidate);
            produse = CalculeazaCaruciorProduse(produse);
            produse = PlatesteCaruciorProduse(produse);

            return produse.Match(
                    whenCaruciorProduseGol: carucioGol => new CaruciorProdusePlatiteFailedEvent("Unexpected unvalidated state") as ICaruciorProdusePlatiteEvent,
                    whenCaruciorProduseNevalidate: caruciorProduseNevalidate => new CaruciorProdusePlatiteFailedEvent("Nu s-au putut valida"),
                    whenCaruciorProduseValidate: produseValidate => new CaruciorProdusePlatiteFailedEvent("Unexpected validated state"),
                    whenCaruciorProduseCalculate: produseCalculate => new CaruciorProdusePlatiteFailedEvent("Unexpected calculated state"),
                    whenCaruciorProdusePlatite: produsePlatite => new CaruciorProdusePlatiteSucceededEvent(produsePlatite.Csv, produsePlatite.PublishedDate)
                );
        }
    }
}
