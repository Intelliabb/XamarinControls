using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Test
{
    public class Keypad : Grid, IDisposable
    {
        #region Fields
        const string CLEAR = "CLR";
        #endregion

        #region Modes
        readonly string[] keys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "", "0", "" };
        readonly string[] currencyKeys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", ".", "0", CLEAR };
        readonly string[] dialpadKeys = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "*", "0", "#" };
        readonly string[] calculatorKeys = { "C", "+/-", "%", "÷", "7", "8", "9", "×", "4", "5", "6", "-", "1", "2", "3", "+", "DEL", "0", ".", "=" };
        #endregion

        #region Events
        public event EventHandler<string> KeyPressed;
        #endregion

        #region Bindable Properties

        public static BindableProperty ModeProperty = BindableProperty.Create(
            nameof(Mode), typeof(Mode), typeof(Keypad), Mode.Default, propertyChanged: ResetView);

        public static BindableProperty KeyStyleProperty = BindableProperty.Create(
            nameof(KeyStyle), typeof(Style), typeof(Keypad), propertyChanged: ResetView);

        public static BindableProperty BackspaceTextProperty = BindableProperty.Create(
            nameof(BackspaceText), typeof(string), typeof(Keypad), propertyChanged: ResetView);

        #endregion

        #region Properties

        public string BackspaceText
        {
            get => (string)GetValue(BackspaceTextProperty);
            set => SetValue(BackspaceTextProperty, value);
        }

        public Mode Mode
        {
            get => (Mode)GetValue(ModeProperty);
            set => SetValue(ModeProperty, value);
        }

        public Style KeyStyle
        {
            get => (Style)GetValue(KeyStyleProperty);
            set => SetValue(KeyStyleProperty, value);
        }

        private static void ResetView(BindableObject bindable, object oldValue, object newValue)
        {
            (bindable as Keypad)?.BuildView();
        }

        #endregion

        public Keypad()
        {
            ColumnSpacing = RowSpacing = 12;
            BuildView();
        }

        void BuildView()
        {
            Children.Clear();
            switch (Mode)
            {
                case Mode.Currency:
                    BuildCurrencyKeypad();
                    break;
                case Mode.Dialpad:
                    BuildDialpad();
                    break;
                case Mode.Calculator:
                    BuildCalculator();
                    break;
                default:
                    BuildKeypad();
                    break;
            }
        }

        void BuildKeypad()
        {
            var queue = new Queue<string>(keys);
            BuildKeys(queue, 4, 3);
        }

        void BuildDialpad()
        {
            var queue = new Queue<string>(dialpadKeys);
            BuildKeys(queue, 4, 3);
        }

        void BuildCurrencyKeypad()
        {
            var queue = new Queue<string>(currencyKeys);
            BuildKeys(queue, 4, 3);
        }

        void BuildCalculator()
        {
            var queue = new Queue<string>(calculatorKeys);
            BuildKeys(queue, 5, 4);
        }

        void BuildKeys(Queue<string> queue, int rows, int columns)
        {
            if (queue.Count < 1)
                return;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    if (queue.Count < 1)
                        break;

                    var key = queue.Dequeue();

                    if (string.IsNullOrWhiteSpace(key))
                        continue;

                    if (key.Equals(CLEAR) && !string.IsNullOrWhiteSpace(BackspaceText))
                        key = BackspaceText;

                    var button = new Button { Text = key, CommandParameter = key, Style = KeyStyle };
                    button.Clicked += Button_Clicked;
                    Children.Add(button, col, row);
                }
            }
        }
        void Button_Clicked(object sender, EventArgs e)
        {
            KeyPressed?.Invoke(this, (sender as Button)?.CommandParameter?.ToString());
        }

        public void Dispose()
        {
            foreach (var key in Children)
            {
                if (key is Button) (key as Button).Clicked -= Button_Clicked;
            }
        }
    }

    public enum Mode
    {
        Default,
        Currency,
        Dialpad,
        Calculator
    }
}
