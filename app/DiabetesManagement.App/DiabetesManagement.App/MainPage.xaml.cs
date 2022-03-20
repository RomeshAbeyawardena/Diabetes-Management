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
            //Source = "file:///android_asset/Content/index.html"
            var localWebView = (WebView)FindByName("localWebView");
            var htmlSource = new HtmlWebViewSource
            {
                Html = indexHtml
            };

            localWebView.Source = htmlSource;
            base.OnAppearing();
        }
    }
}
