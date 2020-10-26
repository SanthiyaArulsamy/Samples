using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Graphics;
using System;

namespace RotationSample
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ImageView imageView;
        Bitmap imageBitmap;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource

            AbsoluteLayout stackView = new AbsoluteLayout(this);
            imageView = new ImageView(this);
            imageView.SetImageResource(Resource.Drawable.Flower1);
            stackView.AddView(imageView);

            LinearLayout stackLayout = new LinearLayout(this);

            Button button = new Button(this) { Text="Rotate"};
            button.Click += Button_Click;

            Button cropbutton = new Button(this) { Text = "Crop" };
            cropbutton.Click += Cropbutton_Click;

            stackLayout.AddView(button);
            stackLayout.AddView(cropbutton);

            stackView.AddView(stackLayout);

            SetContentView(stackView);
        }

        private void Cropbutton_Click(object sender, System.EventArgs e)
        {
            if(imageBitmap == null)
                imageBitmap = ViewToBitmap();
            var cropWindowRect = new System.Drawing.RectangleF(100, 100, 500, 500);

            var points = new float[8]
            {
                    cropWindowRect.Left, cropWindowRect.Top,
                    cropWindowRect.Right, cropWindowRect.Top,
                    cropWindowRect.Right, cropWindowRect.Bottom,
                    cropWindowRect.Left, cropWindowRect.Bottom
            };
            this.imageView.SetImageBitmap(GetCroppedBitmap(imageBitmap, points));
            this.imageView.SetScaleType(ImageView.ScaleType.FitCenter);
            this.imageView.Rotation = 0;
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            if (imageBitmap == null)
                imageBitmap = ViewToBitmap();
            imageView.Rotation = 45;
        }

        internal Bitmap ViewToBitmap()
        {
            var bitmap = Bitmap.CreateBitmap((int)imageView.Width, (int)imageView.Height,
               Bitmap.Config.Argb4444);
            Canvas canvas = new Canvas(bitmap);
            canvas.DrawColor(Color.Transparent);
            imageView.Draw(canvas);
            imageView.Layout(0,0, (int)imageView.Width, (int)imageView.Height);
            return bitmap;
        }

        private static Rect GetRect(float[] points, int imagewidth, int imageheight)
        {
            int left = (int)Math.Round(Math.Max(0, Math.Min(Math.Min(Math.Min(points[0], points[2]), points[4]), points[6])));
            int top = (int)Math.Round(Math.Max(0, Math.Min(Math.Min(Math.Min(points[1], points[3]), points[5]), points[7])));
            int right = (int)Math.Round(Math.Min(imagewidth, Math.Max(Math.Max(Math.Max(points[0], points[2]), points[4]), points[6])));
            int bottom = (int)Math.Round(Math.Min(imageheight, Math.Max(Math.Max(Math.Max(points[1], points[3]), points[5]), points[7])));
            return new Rect(left, top, right, bottom);
        }
        private static Bitmap GetCroppedBitmap(Bitmap bitmap, float[] points)
        {
            Rect rect = GetRect(points, bitmap.Width, bitmap.Height);
            var height = rect.Height();
            var width = rect.Width();
            if (rect.Height() + rect.Top > bitmap.Height)
                height = bitmap.Height - rect.Top;
            if (rect.Width() + rect.Left > bitmap.Width)
                width = bitmap.Width - rect.Left;
            if (width > 0 && height > 0)
                return Bitmap.CreateBitmap(bitmap, rect.Left, rect.Top, width, height);
            else
                return bitmap;
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}