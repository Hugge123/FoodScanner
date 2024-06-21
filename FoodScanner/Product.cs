using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microcharts;
using SkiaSharp;
using System.Globalization;

namespace FoodScanner
{
    public class Product
    {

        public string Name { get; set; }
        public string Brand { get; set; }
        public string Ingredients { get; set; }
        public string NutriScore { get; set; }
        public string ImageSource { get; set; }
        public ChartEntry[] ChartData { get; set; }


        public Product(string initialName, string initialIngredients, string initialNutriscore, string initialBrand, ChartEntry[] initialChartData, string initialImageSource)
        {
            Name = initialName;
            Ingredients = initialIngredients;
            NutriScore = initialNutriscore;
            Brand = initialBrand;
            ChartData = initialChartData;
            ImageSource = initialImageSource;
        }



        public static Product NewProduct(string barcode)
        {
            string name = string.Empty;
            string ingredients = string.Empty;
            string nutriScore = string.Empty;
            string brand = string.Empty;
            string ImageSource = string.Empty;
            ChartEntry[] chartData = null;

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
                    ImageSource = obj.product.image_front_small_url;

                    string initialBrand = obj.product.brands;

                    if (initialBrand.Contains(','))
                    {
                        brand = initialBrand.Split(",")[0];
                    }
                    else
                    {
                        brand = initialBrand;
                    }


                    SKColor[] colors = new SKColor[] {SKColors.Blue, SKColors.Orange, SKColors.Green,
                        SKColors.Red, SKColors.Plum, SKColors.RoyalBlue, SKColors.Cyan, SKColors.Teal,
                        SKColors.Yellow, SKColors.Turquoise, SKColors.Wheat, SKColors.YellowGreen, SKColors.Honeydew,
                SKColors.Gray, SKColors.Bisque, SKColors.Aquamarine, SKColors.Aqua, SKColors.OldLace, SKColors.BurlyWood,
                    SKColors.Crimson, SKColors.MintCream, SKColors.Silver, SKColors.Cornsilk, SKColors.Coral, SKColors.CornflowerBlue};

                    int length = obj.product.ingredients.Count;
                    chartData = new ChartEntry[length - 1];


                    for (int i = 0; i < chartData.Length; i++)
                    {
                        string tmpValue = obj.product.ingredients[i].percent_estimate;

                        var culture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
                        culture.NumberFormat.NumberDecimalSeparator = ".";

                        float value = float.Parse(tmpValue, culture);


                        chartData[i] = new ChartEntry(value)
                        {
                            ValueLabel = $" {Math.Round(value)}% {obj.product.ingredients[i].text}",
                            Color = colors[i],
                            ValueLabelColor = SKColors.Gray
                        };
                    }

                }

                Product newItem = new Product(name, ingredients, nutriScore, brand, chartData, ImageSource);
                Globals.Products.Add(newItem);

                return newItem;
            }catch (Exception e)
            {
                return null;
            }
        }




    }
}

