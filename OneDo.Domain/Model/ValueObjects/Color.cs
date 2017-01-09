using OneDo.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OneDo.Domain.Model.ValueObjects
{
    public class Color : ValueObject<Color>
    {
        public string Hex { get; }

        public Color(string hex)
        {
            Hex = hex;
        }

        public override string ToString()
        {
            return Hex;
        }

        protected override bool EqualsCore(Color other)
        {
            return Hex == other.Hex;
        }

        protected override int GetHashCodeCore()
        {
            return Hex.GetHashCode();
        }
    }
}
