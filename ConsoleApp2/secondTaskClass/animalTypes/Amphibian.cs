using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.secondTaskClass.animalTypes
{
    class Amphibian : Animal
    {
        protected int SkinMoisture { get; }

        public Amphibian(string name, int age, string habitat, string typeNutrition, int skinMoisture) : base(name, age, habitat, typeNutrition)
        {
            SkinMoisture = skinMoisture;
        }

        public override IEnumerable<(string key, object value)> GetInfo()
        {
            foreach (var info in base.GetInfo())
            {
                yield return info;
            }

            yield return (nameof(SkinMoisture), SkinMoisture);
        }
    }
}
