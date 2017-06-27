using GPSImageTag.Core.Models;
using GPSImageTag.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GPSImageTag.ViewModels
{
    public class PhotosPageViewModel : BaseViewModel

    {
        private ObservableCollection<Photo> photos;
        public ObservableCollection<Photo> Photos
        {
            get { return photos; }
            set
            {
                photos = value; OnPropertyChanged("Photos");
            }
        }

        public Command GetPhotosCommand { get; set; }

        public PhotosPageViewModel()
        {
            Title = "Photo List";
            photos = new ObservableCollection<Photo>();
            GetPhotosCommand = new Command(
                async () => await GetPhotos(),
                () => !IsBusy);

        }

        async Task GetPhotos()
        {
            if (IsBusy)
                return;

            Exception error = null;
            try
            {
                IsBusy = true;

                var service = DependencyService.Get<AzureService>();
                var items = await service.GetPhotos();

                Photos.Clear();
                foreach (var item in items)
                    Photos.Add(item);
             }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex);
                error = ex;
            }
            finally
            {
                IsBusy = false;
            }

            if (error != null)
                await Application.Current.MainPage.DisplayAlert("Error!", error.Message, "OK");
        }
    }
}
