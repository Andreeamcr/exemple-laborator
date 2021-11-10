using CSharp.Choices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exemple.Domain
{
    [AsChoice]
    public static partial class Carucior
    {
        public interface ICarucior { }

        public record CaruciorNevalidat(IReadOnlyCollection<ProdusNevalidat> ListaProduse) : ICarucior;

        public record CaruciorGol() : ICarucior;

        public record CaruciorValidat(IReadOnlyCollection<Produs> ListaProduse) : ICarucior;

        public record CaruciorPlatit(IReadOnlyCollection<Produs> ListaProduse, DateTime DataPlata) : ICarucior;
    }
}
