using Plugin.Media;
using System.IO;
using Xamarin.Forms;
using GPSImageTag.Core.Services;
using GPSImageTag.ViewModels;

namespace GPSImageTag.Pages
{
    public partial class CameraPage : ContentPage
    {

        CameraPageViewModel vm;

        public CameraPage()
        {

            InitializeComponent();

            vm = new CameraPageViewModel();

            BindingContext = vm;


        }
    }
}
