using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

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

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        
        async private void InitCameraBtn_Click(object sender,RoutedEventArgs e)
        {
            var photoScan = new PhotoScan();
           await photoScan.StartPreview(CapturePreview);
        }

        private async void CaptureBtn_Click(object sender, RoutedEventArgs e)
        {
            var photoScan = new PhotoScan();
            var scannedCheque =  photoScan.CaptureImage().Result;
            ImagePreview.Source = scannedCheque;
        }
        private void Deposit_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement DepGrid = (sender as FrameworkElement).FindName("DepGrid") as FrameworkElement;
            FrameworkElement A_Cdetais = (sender as FrameworkElement).FindName("A_Cdetais")as FrameworkElement;

            
            if (DepGrid.Visibility = System.Windows.Visibility.Collapsed )
            {
                DepGrid.Visibility = Visibility.Visible;
                A_Cdetais.Visibility = Visibility.Visible;
            }
            else
            {
                DepGrid.Visibility = Visibility.Collapsed;

            }
        }
       async private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new MessageDialog("Work ON Progress");
            await dialog.ShowAsync();  
        }
    }

}
