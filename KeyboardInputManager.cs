using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace Windows_Calculator
{
    public class KeyboardInputManager
    {
        Calculator _calculator;

        public KeyboardInputManager(Calculator calculator)
        {
            _calculator = calculator;
        }

        public void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9)
            {
                NumberKeyPressed(e.Key.ToString().Last().ToString());
                e.Handled = true;
            }
            else if (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)
            {
                NumberKeyPressed((e.Key - Key.NumPad0).ToString());
                e.Handled = true;
            }
            else if (e.Key == Key.Add)
            {
                OperatorKeyPressed("+");
                e.Handled = true;
            }
            else if (e.Key == Key.Subtract)
            {
                OperatorKeyPressed("−");
                e.Handled = true;
            }
            else if (e.Key == Key.Multiply)
            {
                OperatorKeyPressed("×");
                e.Handled = true;
            }
            else if (e.Key == Key.Divide)
            {
                OperatorKeyPressed("÷");
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                _calculator.Equals_Click(sender, new RoutedEventArgs());
                e.Handled = true;
            }
            else if (e.Key == Key.Decimal)
            {
                _calculator.Decimal_Click(sender, new RoutedEventArgs());
                e.Handled = true;
            }
            else if (e.Key == Key.Back)
            {
                _calculator.Backspace_Click(sender, new RoutedEventArgs());
                e.Handled = true;
            }
        }

        private void NumberKeyPressed(string number)
        {
            Button button = new Button { Content = number };
            _calculator.Number_Click(button, null);
        }

        private void OperatorKeyPressed(string operatorSymbol)
        {
            Button button = new Button { Content = operatorSymbol };
            _calculator.Operator_Click(button, new RoutedEventArgs());
        }
    }
}
