using GPSImageTag.Core.Interfaces;
using GPSImageTag.Core.Services;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace GPSImageTag.ViewModels
{
    public class CameraPageViewModel : BaseViewModel
    {

        private IDialogService Dialogs;
        private byte[] imageData;

        public ImageSource Photo
        {
            get {

               return ImageSource.FromStream(() => new MemoryStream(imageData));
            }

        }

        private string imageName;

        public string ImageName
        {
            get { return imageName; }
            set
            {
                imageName = value; OnPropertyChanged("ImageName");
            }
        }

        private string imageDesc;

        public string ImageDesc
        {
            get { return imageDesc; }
            set
            {
                imageDesc = value; OnPropertyChanged("imageDesc");
            }
        }

        public Command TakePhotoCommand { get; set; }
        public Command PickPhotoCommand { get; set; }
        public Command UploadPhotoCommand { get; set; }


        public CameraPageViewModel()
        {

            Dialogs = DependencyService.Get<IDialogService>();

            Title = "Upload Photo";
            imageData = new byte[1];
            TakePhotoCommand = new Command(
                    async () => await TakePhoto(),
                    () => !IsBusy);

            PickPhotoCommand = new Command(
                  async () => await PickPhoto(),
                   () => !IsBusy);


            UploadPhotoCommand = new Command(
                 async () => await UploadPhoto(),
                  () => !IsBusy);

        }

        private async Task UploadPhoto()
        {
            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            var service = DependencyService.Get<AzureService>();

            var name = imageName;
            var desc = imageDesc;

            if (string.IsNullOrEmpty(name))
            {
                Dialogs.ShowError("Enter a photo name before uploading photo");
                IsBusy = false;
                return;
            }

            await service.AddPhoto(new MemoryStream(imageData), name, desc);
            Dialogs.ShowSuccess("File has been successfully uploaded!");
            IsBusy = false;
        }

        private async Task PickPhoto()
        {
            try
            {

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    Dialogs.ShowError("Permission not granted to photos.");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync();

                if (file == null)
                    return;


                var stream = file.GetStream();
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.Position = 0; // needed for WP (in iOS and Android it also works without it)!!
                    stream.CopyTo(ms);  // was empty without stream.Position = 0;
                    imageData = ms.ToArray();
                }
                OnPropertyChanged("Photo");
                file.Dispose();
            }
            catch (Exception ex)
            {
                Dialogs.ShowError("Unable to pick a photo: " + ex);
            }
        }

        private async Task TakePhoto()
        {
            try
            {
                await CrossMedia.Current.Initialize();


                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    Dialogs.ShowError("No camera avaialble.");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {

                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                Dialogs.ShowSuccess(file.Path);

                var stream = file.GetStream();
                using (MemoryStream ms = new MemoryStream())
                {
                    stream.Position = 0; // needed for WP (in iOS and Android it also works without it)!!
                    stream.CopyTo(ms);  // was empty without stream.Position = 0;
                    imageData = ms.ToArray();
                }
                OnPropertyChanged("Photo");
                file.Dispose();
            }
            catch (Exception ex)
            {
                Dialogs.ShowError("Unable to camera capabilities: " + ex);
            }
        }
    }
}
