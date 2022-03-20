using Xamarin.Forms;

namespace DiabetesManagement.App
{
    public partial class MainPage : ContentPage
    {
        private readonly string indexHtml;
        public MainPage(string indexHtml)
        {
            this.indexHtml = indexHtml;
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}
