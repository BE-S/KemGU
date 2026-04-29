using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.secondTaskClass.animalTypes
{
    class Fish : Animal
    {
        protected string WaterType;

        public Fish(string name, int age, string habitat, string typeNutrition, string waterType) : base(name, age, habitat, typeNutrition)
        {
            WaterType = waterType;
        }

        public override IEnumerable<(string key, object value)> GetInfo()
        {
            foreach (var info in base.GetInfo())
            {
                yield return info;
            }

            yield return (nameof(WaterType), WaterType);
        }
    }
}
