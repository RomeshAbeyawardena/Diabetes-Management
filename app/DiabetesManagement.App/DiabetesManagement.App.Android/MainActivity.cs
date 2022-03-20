using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using DiabetesManagement.App.Shared.Services;

namespace DiabetesManagement.App.Droid
{
    [Activity(Label = "DiabetesManagement.App", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private readonly AssetService assetStreamService = new AssetService();
        private const string IndexHtmlAsset = "Content/index.html";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            var indexHtml = assetStreamService.GetAsset(IndexHtmlAsset, r => Assets.Open(r));
            var resources = assetStreamService.ParseRequiredResources(indexHtml);
            var assets = assetStreamService.GetResources(resources, r => Assets.Open(r));
            indexHtml = assetStreamService.ReplaceResources(indexHtml, assets, r => Assets.Open(r));

            LoadApplication(new App(indexHtml));
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}