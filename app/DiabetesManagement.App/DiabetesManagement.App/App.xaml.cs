using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace DiabetesManagement.App
{
    public partial class App : Application
    {
        public App(string indexHtml)
        {
            InitializeComponent();

            MainPage = new MainPage(indexHtml);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
