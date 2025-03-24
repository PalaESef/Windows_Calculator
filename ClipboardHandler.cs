using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Windows_Calculator
{
    public class ClipboardHandler
    {
        private readonly TextBox _display;

        public ClipboardHandler(TextBox display)
        {
            _display = display;
        }
        public void HandleCut(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_display.Text);
            _display.Text = "0";
        }
        public void HandleCopy(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(_display.Text);
        }

        public void HandlePaste(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(Clipboard.GetText(), out double value))
            {
                _display.Text = value.ToString(CultureInfo.CurrentCulture);
            }
        }

        public void HandleAbout(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Calculator WPF\nRealizat de: Palaghia Andrei\nGrupa: 332",
                            "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
