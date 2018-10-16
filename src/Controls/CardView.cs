using Xamarin.Forms;

namespace IntelliAbb.Xamarin.Controls
{
    public class CardView : Frame
    {
        #region Control Templates
        readonly ControlTemplate _iconTitleTemplate = new ControlTemplate(typeof(IconTitleTemplate));
        readonly ControlTemplate _titleTemplate = new ControlTemplate(typeof(TitleTemplate));
        #endregion

        public CardView()
        {
            CornerRadius = 8;
            Padding = new Thickness(8);
        }

        #region Defaults
        static Style DEFAULT_TITLE_STYLE
        {
            get
            {
                return new Style(typeof(Label))
                {
                    Setters =
                        {
                            new Setter
                            {
                                Property = Label.FontAttributesProperty, Value=FontAttributes.Bold
                            },
                            new Setter
                            {
                                Property = Label.FontSizeProperty, Value= 14
                            },
                            new Setter
                            {
                                Property = Label.TextColorProperty, Value = Color.Gray
                            },
                            new Setter
                            {
                                Property = Label.VerticalTextAlignmentProperty, Value = TextAlignment.Center
                            }
                        }
                };
            }
        }

        static Style DEFAULT_ICON_STYLE
        {
            get
            {
                return new Style(typeof(Image))
                {
                    Setters =
                        {
                            new Setter
                            {
                                Property = Image.WidthRequestProperty, Value=24
                            },
                            new Setter
                            {
                                Property = Image.HeightRequestProperty, Value=24
                            },
                            new Setter
                            {
                                Property = Image.AspectProperty, Value=Aspect.AspectFit
                            }
                        }
                };
            }
        }
        #endregion

        #region Bindable Properties
        public static BindableProperty TitleProperty = BindableProperty.Create(nameof(Title),
            typeof(string),
            typeof(CardView),
            propertyChanged: OnTitleChanged);

        public static BindableProperty IconProperty = BindableProperty.Create(nameof(Icon),
            typeof(ImageSource),
            typeof(CardView),
            propertyChanged: OnTitleChanged);

        public static BindableProperty TitleStyleProperty = BindableProperty.Create(nameof(TitleStyle),
            typeof(Style),
            typeof(CardView),
            DEFAULT_TITLE_STYLE);

        public static BindableProperty IconStyleProperty = BindableProperty.Create(nameof(IconStyle),
            typeof(Style),
            typeof(CardView),
            DEFAULT_ICON_STYLE);

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public Style TitleStyle
        {
            get { return (Style)GetValue(TitleStyleProperty); }
            set { SetValue(TitleStyleProperty, value); }
        }

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public Style IconStyle
        {
            get { return (Style)GetValue(IconStyleProperty); }
            set { SetValue(IconStyleProperty, value); }
        }

        private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is CardView card)) return;
            card.ControlTemplate = card.Icon != null ? card._iconTitleTemplate : card._titleTemplate;
        }
        #endregion
    }

    /// <summary>
    /// Control template for Icon and Title.
    /// </summary>
    public class IconTitleTemplate : ContentView
    {
        public IconTitleTemplate()
        {
            var titleLabel = new Label();
            titleLabel.SetBinding(Label.TextProperty, new TemplateBinding("Title"));
            titleLabel.SetBinding(Label.StyleProperty, new TemplateBinding("TitleStyle"));

            Image icon = new Image();
            icon.SetBinding(Image.SourceProperty, new TemplateBinding("Icon"));
            icon.SetBinding(Image.StyleProperty, new TemplateBinding("IconStyle"));

            Content = new StackLayout
            {
                Children =
                {
                    new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            icon,
                            titleLabel
                        }
                    },
                    new ContentPresenter(),
                }
            };
        }
    }

    /// <summary>
    /// Control template for Title only.
    /// </summary>
    public class TitleTemplate : ContentView
    {
        public TitleTemplate()
        {
            var titleLabel = new Label();
            titleLabel.SetBinding(Label.TextProperty, new TemplateBinding("Title"));
            titleLabel.SetBinding(Label.StyleProperty, new TemplateBinding("TitleStyle"));

            Content = new StackLayout
            {
                Children =
                {
                    titleLabel,
                    new ContentPresenter(),
                }
            };
        }
    }
}
