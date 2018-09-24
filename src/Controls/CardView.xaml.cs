using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace IntelliAbb.Xamarin.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CardView : Frame
	{
		public CardView ()
		{
			InitializeComponent ();
		}
        #region Bindable Properties

        #region Icon

        public static BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(ImageSource), typeof(CardView), propertyChanged: IconTitleGridVisible);

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        #endregion

        #region Title

        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(CardView), propertyChanged: IconTitleGridVisible);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        #endregion

        #region TitleStyle

        public static BindableProperty TitleStyleProperty = BindableProperty.Create(nameof(TitleStyle), typeof(Style), typeof(CardView), default(Style));

        public Style TitleStyle
        {
            get { return (Style)GetValue(TitleStyleProperty); }
            set { SetValue(TitleStyleProperty, value); }
        }

        #endregion

        #region Main Content

        public static BindableProperty CardContentProperty = BindableProperty.Create(nameof(CardContent), typeof(View), typeof(CardView), propertyChanged: IconTitleGridVisible);

        public View CardContent
        {
            get { return (View)GetValue(CardContentProperty); }
            set { SetValue(CardContentProperty, value); }
        }

        #endregion

        #endregion


        #region Property Changed Methods

        private static void IconTitleGridVisible(BindableObject bindable, object oldValue, object newValue)
        {
            var view = (CardView)bindable;

            view.IconTitleGrid.IsVisible = (!string.IsNullOrEmpty(view.Title) || view.Icon != null);
        }

        #endregion
    }
}