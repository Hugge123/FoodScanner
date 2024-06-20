using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;

namespace FoodScanner
{
    public class Product
    {

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Ingredients { get; set; }
        public string NutriScore { get; set; }


        public Product(string initialName, string initialIngredients, string initialNutriscore, string initialBrand)
        {
            Name = initialName;
            Ingredients = initialIngredients;
            NutriScore = initialNutriscore;
            Brand = initialBrand;
        }



        public static Product NewProduct(string barcode)
        {
            string name = string.Empty;
            string ingredients = string.Empty;
            string nutriScore = string.Empty;
            string brand = string.Empty;

            try
            {
                using (var client = new HttpClient())
                {
                    Uri endpoint = new Uri($"https://openfoodfacts.org/api/v0/product/{barcode}");
                    var result = client.GetAsync(endpoint).Result;
                    var json = result.Content.ReadAsStringAsync().Result;

                    dynamic obj = JsonConvert.DeserializeObject(json);

                    if (obj.status_verbose == "product not found")
                    {
                        return null;
                    }

                    name = obj.product.product_name;
                    ingredients = obj.product.ingredients_text;
                    nutriScore = obj.product.nutriscore_grade;


                    string initialBrand = obj.product.brands;

                    if (initialBrand.Contains(","))
                    {
                        brand = initialBrand.Split(",")[0];
                    }
                    else
                    {
                        brand = initialBrand;
                    }
                }


                Product newItem = new Product(name, ingredients, nutriScore, brand);
                Globals.Products.Add(newItem);

                return newItem;
            }
            catch (Exception ex)
            {
                return null;
            }


        }
    }
}

