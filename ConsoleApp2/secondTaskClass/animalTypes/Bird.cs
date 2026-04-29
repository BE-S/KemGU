using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2.secondTaskClass.animalTypes
{
    class Bird : Animal
    {
        protected double WingSpan { get; }

        public Bird(string name, int age, string habitat, string typeNutrition, double wingSpan) : base(name, age, habitat, typeNutrition)
        {
            WingSpan = wingSpan;
        }

        public override IEnumerable<(string key, object value)> GetInfo()
        {
            foreach (var info in base.GetInfo())
            {
                yield return info;
            }

            yield return (nameof(WingSpan), WingSpan);
        }
    }
}
