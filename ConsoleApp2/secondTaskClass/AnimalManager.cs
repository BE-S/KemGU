using System;
using System.Linq;
using System.Collections.Generic;

namespace ConsoleApp2.secondTaskClass
{
    class Product
    {
        public Animal Animal { get; }
        public double Price { get; }
        public DateTime DeliveryDate { get; }
        public DateTime ExpirationDate { get; }

        public Product(Animal animal)
        {
            Animal = animal;
        }
    }

    class AnimalManager
    {
        private static AnimalManager Instance;
        protected List<Product> _products { get; } = new List<Product>{};

        private AnimalManager() {}

        public static AnimalManager getInstance()
        {
            if (Instance == null)
            {
                Instance = new AnimalManager();
            }

            return Instance;
        }

        public void setProduct(Animal animal)
        {
            _products.Add(
                new Product(animal)
            );
        }

        public void setProducts(params Animal[] animals)
        {
            foreach (var animal in animals)
            {
                _products.Add(
                    new Product(animal)
                );
            }
        }

        public void getProducts()
        {
            foreach (Product product in _products)
            {
                showProduct(product);
            }
        }

        public void findProductsByIndex(int index)
        {
            if (index >= 0 && index < _products.Count)
            {
                Product product = _products[index];

                showProduct(product);
            }
            else
            {
                productNotFind();
            }
        }

        public void findProductByName(string name)
        {
            Product product = _products.FirstOrDefault(p => p.Animal?.getName().ToLower() == name.ToLower());

            showProduct(product);
        }

        private void showProduct(Product product)
        {
            if (product == null)
            {
                Console.WriteLine("Товар не найден\n");
                return;
            }

            Console.WriteLine("Animal type: {0}", product.Animal.GetType().Name);

            foreach (var (key, value) in product.Animal.GetInfo())
            {
                Console.WriteLine("{0}: {1}", key, value);
            }
            Console.WriteLine("______________________");
        }

        private void productNotFind()
        {
            Console.WriteLine("Извините. Товар не найден");
        }
    }
}
