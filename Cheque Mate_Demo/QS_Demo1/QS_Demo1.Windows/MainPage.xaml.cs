using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using WindowsPreview.Media.Ocr;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace QS_Demo1
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void CapturePhoto_Click(object sender, RoutedEventArgs e)
        {
            PhotoScan photoScan = new PhotoScan();
            StorageFile capturedImagePath = await photoScan.CaptureClick(TakenImage);


            var recognizedWords = await GetTextAsync(capturedImagePath);

            TextBoxAcntHldrName.Text = recognizedWords[0];
        }


        private async Task<List<string>> GetTextAsync(StorageFile capturedImageFile)
        {
            OcrEngine _engine = new OcrEngine(OcrLanguage.English);
            //var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;



            var imageProperties = await capturedImageFile.Properties.GetImagePropertiesAsync();

            //var storageFile = StorageFile.GetFileFromPathAsync(capturedImagePath);
            //var storageFile = await StorageFile.GetFileFromPathAsync(capturedImagePath);

            using (var imgStream = await capturedImageFile.OpenAsync(FileAccessMode.Read))
            {
                var bitmap = new WriteableBitmap((int)imageProperties.Width, (int)imageProperties.Height);
                bitmap.SetSource(imgStream);
                //bitmap.SetSourceAsync(imgStream);
                OcrResult ocrExtract = await
                        _engine.RecognizeAsync((uint)bitmap.PixelHeight, (uint)bitmap.PixelWidth,
                            bitmap.PixelBuffer.ToArray());
                var recognizedWords = new List<string>();
                if (ocrExtract.Lines != null)
                {
                    recognizedWords.AddRange(ocrExtract.Lines.SelectMany(line => line.Words)
                                                             .Select(word => word.Text));
                }
                return recognizedWords;
            }
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
           PrgRing.Visibility = Visibility.Visible;
           await System.Threading.Tasks.Task.Delay(TimeSpan.FromSeconds(5));
           var dialog = new MessageDialog("Data Send to the bank");
           await dialog.ShowAsync();
            PrgRing.Visibility = Visibility.Collapsed;

        }

        private void DepClick(object sender, RoutedEventArgs e)
        {
            DePanel.Visibility = Visibility.Visible;
            Grd2.Visibility = Visibility.Visible;

        }

        async private void NxtWrkButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Work ON Progress");
            await dialog.ShowAsync();  
        }
    }
}

