using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain.Models
{
    [AsChoice]
    public static partial class CaruciorProduse
    {
        public interface ICaruciorProduse { }

        public record CaruciorProduseGol: ICaruciorProduse
        {
            public IReadOnlyCollection<Produs> ListaProduse { get; }
        }

        public record CaruciorProduseNevalidate: ICaruciorProduse
        {
            internal CaruciorProduseNevalidate(IReadOnlyCollection<ProdusNevalidat> listaProduse)
            {
                ListaProduse = listaProduse;
            }

            public IReadOnlyCollection<ProdusNevalidat> ListaProduse { get; }
        }

        public record CaruciorProduseValidate: ICaruciorProduse
        {
            internal CaruciorProduseValidate(IReadOnlyCollection<ProdusValidat> listaProduse)
            {
                ListaProduse = listaProduse;
            }

            public IReadOnlyCollection<ProdusValidat> ListaProduse { get; }
        }

        public record CaruciorProduseCalculate : ICaruciorProduse
        {
            internal CaruciorProduseCalculate(IReadOnlyCollection<ProdusCalculat> listaProduse)
            {
                ListaProduse = listaProduse;
            }

            public IReadOnlyCollection<ProdusCalculat> ListaProduse { get; }
        }

        public record CaruciorProdusePlatite : ICaruciorProduse
        {
            internal CaruciorProdusePlatite(IReadOnlyCollection<ProdusCalculat> listaProduse, string csv, DateTime publishedDate)
            {
                ListaProduse = listaProduse;
                PublishedDate = publishedDate;
                Csv = csv;
            }

            public IReadOnlyCollection<ProdusCalculat> ListaProduse { get; }
            public DateTime PublishedDate { get; }
            public string Csv { get; }
        }
    }
}
