using System.Threading;
using ZXing;
namespace FoodScanner;

public partial class Scanner : ContentPage
{
    public Scanner()
    {
        InitializeComponent();

        cameraView.BarCodeOptions = new Camera.MAUI.ZXingHelper.BarcodeDecodeOptions()
        {
            AutoRotate = true,
            PossibleFormats = { BarcodeFormat.EAN_13, BarcodeFormat.EAN_8},
            ReadMultipleCodes = false,
            TryHarder = true,
            TryInverted = true
        };

        cameraView.StartCameraAsync();
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private void CameraView_BarcodeDetected(object sender, Camera.MAUI.ZXingHelper.BarcodeEventArgs args)
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
           await cameraView.StopCameraAsync();


            string barcode = args.Result[0].Text;
            
            var scannedProduct = Product.NewProduct(barcode);

            if (scannedProduct == null)
            {
                Dispatcher.Dispatch(async () =>
                {
                    await DisplayAlert("Info", "Product Not Found", "OK");
                });
                Thread.Sleep(50);
                await cameraView.StartCameraAsync();
            }
            else
            {

                Globals.ActiveProduct = scannedProduct;
                await Navigation.PushAsync(new ProductPage());
            }
            


        });
    }
}