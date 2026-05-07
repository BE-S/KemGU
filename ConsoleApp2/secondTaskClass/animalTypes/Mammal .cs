using System.Collections.Generic;

namespace ConsoleApp2.secondTaskClass.animalTypes
{
    class Mammal : Animal
    {
        protected bool HarFur { get; }

        public Mammal(string name, int age, string habitat, string typeNutrition, bool hasFur) : base(name, age, habitat, typeNutrition)
        {
            HarFur = HarFur;
        }

        public override IEnumerable<(string key, object value)> GetInfo()
        {
            foreach (var info in base.GetInfo())
            {
                yield return info;
            }

            yield return (nameof(HarFur), HarFur);
        }
    }
}
