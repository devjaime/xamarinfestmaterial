using Android.Widget;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
using GPSImageTag.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(Xamarin.Forms.Button), typeof(FlatButtonRenderer))]
namespace GPSImageTag.Droid.CustomRenderers
{
    public class FlatButtonRenderer : ButtonRenderer
    {
        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }
    }
}
