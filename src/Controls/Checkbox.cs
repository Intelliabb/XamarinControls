using System;
using System.Collections.Generic;
using Xamarin.Forms;
using SkiaSharp.Views.Forms;
using SkiaSharp;
using System.ComponentModel;
using System.Windows.Input;

namespace IntelliAbb.Xamarin.Controls
{
    /// <summary>
    /// This is a cross-platform checkbox control.
    /// </summary>
    [Browsable(true)]
    public class Checkbox : ContentView, IDisposable
    {
        #region Fields

        const double DEFAULT_SIZE = 28;
        bool isAnimating;
        SKCanvasView skiaView;
        ICommand ToggleCommand;

        #endregion

        #region Constructor

        public Checkbox()
        {
            InitializeCanvas();
            Content = skiaView;
        }

        #endregion

        #region Initialize Canvas

        void InitializeCanvas()
        {
            ToggleCommand = new Command(OnTappedCommand);

            skiaView = new SKCanvasView();
            skiaView.PaintSurface += Handle_PaintSurface;
            skiaView.WidthRequest = skiaView.HeightRequest = DEFAULT_SIZE;
            skiaView.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = ToggleCommand
            });
        }

        async void OnTappedCommand(object obj)
        {
            if (isAnimating)
                return;

            isAnimating = true;
            IsChecked = !IsChecked;

            if (!IsChecked)
                skiaView.InvalidateSurface();
            else
            {
                await skiaView.ScaleTo(0.85, 100);
                skiaView.InvalidateSurface();
                await skiaView.ScaleTo(1, 100, IsChecked ? Easing.BounceOut : null);
            }
            isAnimating = false;
        }
        #endregion

        #region Checkmark Paint Surface
        void Handle_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            e?.Surface?.Canvas?.Clear();
            
            if (IsChecked)
            {
                DrawCheckFilled(e);
            }
            else
            {
                DrawOutline(e);
            }
        }

        void DrawCheckFilled(SKPaintSurfaceEventArgs e)
        {
            var imageInfo = e.Info;
            var canvas = e?.Surface?.Canvas;

            using (var checkfill = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = FillColor.ToSKColor(),
                StrokeJoin = SKStrokeJoin.Round
            })
            {
                canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, (float)((imageInfo.Width / 2) - OutlineWidth), checkfill);
            }

            using (var checkPath = new SKPath())
            {
                checkPath.MoveTo(.25f * imageInfo.Width, .53f * imageInfo.Height);
                checkPath.LineTo(.4f * imageInfo.Width, .7f * imageInfo.Height);
                checkPath.LineTo(.75f * imageInfo.Width, .4f * imageInfo.Height);

                using (var checkStroke = new SKPaint
                {
                    Style = SKPaintStyle.Stroke,
                    Color = CheckColor.ToSKColor(),
                    StrokeWidth = (float)OutlineWidth,
                    StrokeCap = SKStrokeCap.Round
                })
                {
                    canvas.DrawPath(checkPath, checkStroke);
                }
            }
        }

        void DrawOutline(SKPaintSurfaceEventArgs e)
        {

            var imageInfo = e.Info;
            var canvas = e?.Surface?.Canvas;

            using (var outline = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = OutlineColor.ToSKColor(),
                StrokeWidth = (float)OutlineWidth,
                StrokeJoin = SKStrokeJoin.Round
            })
            {
                canvas.DrawCircle(imageInfo.Width / 2, imageInfo.Height / 2, (float)((imageInfo.Width / 2) - OutlineWidth), outline);
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// Occurs when IsChecked is changed.
        /// </summary>
        public event EventHandler<TappedEventArgs> IsCheckedChanged;
        #endregion

        #region Bindable Properties
        public static BindableProperty OutlineColorProperty = BindableProperty.Create(nameof(OutlineColor), typeof(Color), typeof(Checkbox), Color.Blue);

        /// <summary>
        /// Gets or sets the color of the outline.
        /// </summary>
        /// <value>Xamarin.Forms.Color value of the outline</value>
        public Color OutlineColor
        {
            get { return (Color)GetValue(OutlineColorProperty); }
            set { SetValue(OutlineColorProperty, value); }
        }

        public static BindableProperty FillColorProperty = BindableProperty.Create(nameof(FillColor), typeof(Color), typeof(Checkbox), Color.Blue);
        /// <summary>
        /// Gets or sets the color of the fill.
        /// </summary>
        /// <value>Xamarin.Forms.Color value of the fill.</value>
        public Color FillColor
        {
            get { return (Color)GetValue(FillColorProperty); }
            set { SetValue(FillColorProperty, value); }
        }

        public static BindableProperty CheckColorProperty = BindableProperty.Create(nameof(CheckColor), typeof(Color), typeof(Checkbox), Color.White);
        /// <summary>
        /// Gets or sets the color of the check.
        /// </summary>
        /// <value>Xamarin.Forms.Color value of the check.</value>
        public Color CheckColor
        {
            get { return (Color)GetValue(CheckColorProperty); }
            set { SetValue(CheckColorProperty, value); }
        }

        public static BindableProperty OutlineWidthProperty = BindableProperty.Create(nameof(OutlineWidth), typeof(double), typeof(Checkbox), 8.0);
        /// <summary>
        /// Gets or sets the width of the outline and check.
        /// </summary>
        /// <value>The width of the outline and check.</value>
        public double OutlineWidth
        {
            get { return (double)GetValue(OutlineWidthProperty); }
            set { SetValue(OutlineWidthProperty, value); }
        }

        public static new BindableProperty StyleProperty = BindableProperty.Create(nameof(Style), typeof(Style), typeof(Checkbox), propertyChanged: OnStyleChanged);

        /// <summary>
        /// Gets or sets the style for HCTRACheckbox.
        /// </summary>
        /// <value>The style.</value>
        public new Style Style
        {
            get { return (Style)GetValue(StyleProperty); }
            set { SetValue(StyleProperty, value); }
        }

        static void OnStyleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Checkbox checkbox)) return;

            var setters = ((Style)newValue).Setters;
            var dict = new Dictionary<string, Color>();

            foreach (var setter in setters)
            {
                dict.Add(setter.Property.PropertyName, (Color)setter.Value);
            }

            checkbox.OutlineColor = dict[nameof(OutlineColor)];
            checkbox.FillColor = dict[nameof(FillColor)];
            checkbox.CheckColor = dict[nameof(CheckColor)];
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(Checkbox), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="T:HCTRAMobile.Controls.HCTRACheckbox"/> is checked.
        /// </summary>
        /// <value><c>true</c> if is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var checkbox = bindable as Checkbox;
            checkbox?.IsCheckedChanged?.Invoke(checkbox, new TappedEventArgs((bool)newValue));
            checkbox?.skiaView?.InvalidateSurface();
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            skiaView.PaintSurface -= Handle_PaintSurface;
        }

        #endregion
    }
}

