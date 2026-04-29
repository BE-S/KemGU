using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.secondTaskClass.animalTypes
{
    class Reptile : Animal
    {
        protected bool IsVenomous { get; }

        public Reptile(string name, int age, string habitat, string typeNutrition, bool isVenomous) : base(name, age, habitat, typeNutrition)
        {
            IsVenomous = isVenomous;
        }

        public override IEnumerable<(string key, object value)> GetInfo()
        {
            foreach (var info in base.GetInfo())
            {
                yield return info;
            }

            yield return (nameof(IsVenomous), IsVenomous);
        }
    }
}
