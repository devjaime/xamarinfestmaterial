using GPSImageTag.ViewModels;
using Xamarin.Forms;

namespace GPSImageTag.Pages
{
    public partial class PhotosPage : ContentPage
    {
        PhotosPageViewModel vm;
        public PhotosPage()
        {
            InitializeComponent();

            vm = new PhotosPageViewModel();

            BindingContext = vm;
        }
    }
}
