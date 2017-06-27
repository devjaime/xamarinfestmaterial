using GPSImageTag.Core.Interfaces;
using GPSImageTag.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GPSImageTag.Core.Helpers;
using Xamarin.Forms;

namespace GPSImageTag
{
    public partial class App : Application
    {

        public App()
        {

            InitializeComponent();

            var navigationPage = new NavigationPage(new StartPage())
            {
                BarTextColor = Colours.NavigationBarTextColor,
                BarBackgroundColor = Colours.BackgroundColor,
            };


            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
