namespace FoodScanner;

public partial class ProductPage : ContentPage
{
    public ProductPage()
    {
        InitializeComponent();


        productName.Text = Globals.ActiveProduct.Name;
        productIngredients.Text = Globals.ActiveProduct.Ingredients;
        productBrand.Text = Globals.ActiveProduct.Brand;


        nutriScoreImg.IsVisible = true;
        switch (Globals.ActiveProduct.NutriScore)
        {
            case "a":
                nutriScoreImg.Source = "./Images/nutriscore_a.svg";
                break;
            case "b":
                nutriScoreImg.Source = "./Images/nutriscore_b.svg";
                break;
            case "c":
                nutriScoreImg.Source = "./Images/nutriscore_c.svg";
                break;
            case "d":
                nutriScoreImg.Source = "./Images/nutriscore_d.svg";
                break;
            case "e":
                nutriScoreImg.Source = "./Images/nutriscore_e.svg";
                break;
            default:
                nutriScoreImg.IsVisible = false;
                break;
        }
    }
}