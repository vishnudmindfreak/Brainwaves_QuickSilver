using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace QS_Demo1
{
    class PhotoScan
    {

#if WINDOWS_PHONE_APP
        Windows.Media.Capture.MediaCapture captureManager;
        private static async Task<DeviceInformation> GetCameraID(Windows.Devices.Enumeration.Panel desiredCamera)
        {
            DeviceInformation deviceID = (await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture))
                .FirstOrDefault(x => x.EnclosureLocation != null && x.EnclosureLocation.Panel == desiredCamera);
            if (deviceID != null)
            {
                return deviceID;
            }
            else throw new Exception(string.Format("Camera of type {0} does not exist", desiredCamera));
        }

        public async Task StartPreview(CaptureElement captureElement)
        {
            var cameraId = await GetCameraID(Windows.Devices.Enumeration.Panel.Back);
            captureManager = new MediaCapture();
            await captureManager.InitializeAsync(new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Video,
                    PhotoCaptureSource = PhotoCaptureSource.VideoPreview,
                    AudioDeviceId = string.Empty,
                    VideoDeviceId = cameraId.Id
                });
            var maxResolution = captureManager.VideoDeviceController.GetAvailableMediaStreamProperties(MediaStreamType.Photo).Aggregate((i1, i2) => (i1 as VideoEncodingProperties).Width > (i2 as VideoEncodingProperties).Width ? i1 : i2);
            await captureManager.VideoDeviceController.SetMediaStreamPropertiesAsync(MediaStreamType.Photo, maxResolution);
            captureElement.Source = captureManager;
            await captureManager.StartPreviewAsync();
        }

        public async Task<BitmapImage> CaptureImage()
        {
            var imgFormat = ImageEncodingProperties.CreateBmp();


            var file = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("TestPhoto.bmp",CreationCollisionOption.GenerateUniqueName);

            //var stream = new InMemoryRandomAccessStream();
            await captureManager.CapturePhotoToStorageFileAsync(imgFormat, file);
            //var photoTask= captureManager.CapturePhotoToStreamAsync(imgFormat, stream);
            

            var bmpImage = new BitmapImage(new Uri(file.Path));
            //var bmpImage = new BitmapImage();
            //bmpImage.SetSource(file);

            return bmpImage;

        }



#else
        public async Task<StorageFile> CaptureClick(Image takenimg)
        {
        var Camera = new CameraCaptureUI();
            var img = await Camera.CaptureFileAsync(CameraCaptureUIMode.Photo);
            
            if (img != null)
            {
                var stream = await img.OpenAsync(FileAccessMode.Read);
                var bitmap = new BitmapImage();
                bitmap.SetSource(stream);
               takenimg.Source = bitmap;
               return img;
            }
            else
            {
                var dialog = new MessageDialog("The user has not taken the photo");
                await dialog.ShowAsync();
                return null;
            }
        }

#endif
        //private async Task GetTextAsync()
        //{
        //    OcrEngine _engine = new OcrEngine(OcrLanguage.English);
        //    var folder = Windows.ApplicationModel.Package.Current.InstalledLocation;
        //    var imageFile = await folder.GetFileAsync("img.jpg");

        //    var imageProperties = await imageFile.Properties.GetImagePropertiesAsync();

        //    using (var imgStream = await imageFile.OpenAsync(FileAccessMode.Read))
        //    {
        //        var bitmap = new WriteableBitmap((int)imageProperties.Width, (int)imageProperties.Height);
        //        bitmap.SetSource(imgStream);

        //        OcrResult ocrExtract = await
        //                _engine.RecognizeAsync((uint)bitmap.PixelHeight, (uint)bitmap.PixelWidth,
        //                    bitmap.PixelBuffer.ToArray());
        //        foreach (OcrWord word in ocrExtract.Lines.SelectMany(line => line.Words))
        //        {
        //            Message += string.Format(" {0}", word.Text);
        //        }
        //    }

        //}
    }
}



