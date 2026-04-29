using System;
using System.Collections.Generic;

namespace ConsoleApp2.secondTaskClass
{
    abstract class Animal
    {
        protected string Name { get; }
        protected int Age { get; }
        protected string Habitat { get; }
        protected string TypeNutrition { get; }

        public Animal(string name, int age, string habitat, string typeNutrition)
        {
            Name          = name;
            Age           = age;
            Habitat       = habitat;
            TypeNutrition = typeNutrition;
        }

        virtual public IEnumerable<(string key, object value)> GetInfo()
        {
            yield return (nameof(Name), Name);
            yield return (nameof(Age), Age);
            yield return (nameof(Habitat), Habitat);
            yield return (nameof(TypeNutrition), TypeNutrition);
        }

        public string getName()
        {
            return Name;
        }
    }
}
