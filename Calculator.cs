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
    public class Calculator
    {
        private double firstNumber = 0;
        private string operation = "";
        private bool isNewEntry = true;
        private List<string> history = new List<string>();
        private TextBox _display;

        public Calculator(TextBox display)
        {
            _display = display;
        }

        public void Equals_Click(object sender, RoutedEventArgs e)
        {
            double secondNumber = double.Parse(_display.Text);
            double result = 0;

            switch (operation)
            {
                case "+":
                    result = firstNumber + secondNumber;
                    break;
                case "−":
                    result = firstNumber - secondNumber;
                    break;
                case "×":
                    result = firstNumber * secondNumber;
                    break;
                case "÷":
                    if (secondNumber != 0)
                        result = firstNumber / secondNumber;
                    else
                    {
                        _display.Text = "Error";
                        return;
                    }
                    break;
            }

            string formattedResult;
            if (result % 1 == 0)
            {
                formattedResult = result.ToString("N0", CultureInfo.CurrentCulture);
            }
            else
            {
                formattedResult = result.ToString("N2", CultureInfo.CurrentCulture);
            }

            string historyEntry = $"{firstNumber} {operation} {secondNumber} = {formattedResult}";
            history.Add(historyEntry);

            _display.Text = formattedResult;

            firstNumber = result;
            operation = "";
            isNewEntry = true;
        }


        public void Number_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (isNewEntry || _display.Text == "0")
                {
                    _display.Text = button.Content.ToString();
                    isNewEntry = false;
                }
                else
                {
                    _display.Text += button.Content.ToString();
                }

                FormatDisplayText();
            }
            else
            {
                Console.WriteLine("Error: sender is not a Button");
            }
        }



        public void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (!double.TryParse(_display.Text, out double currentNumber))
                {
                    _display.Text = "Error";
                    return;
                }

                if (!string.IsNullOrEmpty(operation))
                {
                    switch (operation)
                    {
                        case "+":
                            firstNumber += currentNumber;
                            break;
                        case "−": // Fancy Minus Symbol
                        case "-":
                            firstNumber -= currentNumber;
                            break;
                        case "×": // Fancy Multiply Symbol
                        case "*":
                            firstNumber *= currentNumber;
                            break;
                        case "÷": // Fancy Divide Symbol
                        case "/":
                            if (currentNumber != 0)
                                firstNumber /= currentNumber;
                            else
                            {
                                _display.Text = "Error";
                                return;
                            }
                            break;
                    }
                }
                else
                {
                    firstNumber = currentNumber;
                }

                _display.Text = firstNumber.ToString("N0", CultureInfo.CurrentCulture);
                operation = button.Content.ToString();
                isNewEntry = true;
            }
        }


        public void FormatDisplayText()
        {
            try
            {
                if (_display.Text.Contains("."))
                {
                    return;
                }

                decimal number = decimal.Parse(_display.Text);

                _display.Text = number.ToString("N0", CultureInfo.CurrentCulture);
            }
            catch (FormatException)
            {
            }
        }

        public void Clear_Click(object sender, RoutedEventArgs e)
        {
            _display.Text = "0";
            firstNumber = 0;
            operation = "";
            isNewEntry = true;
        }

        public void History_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count == 0)
            {
                MessageBox.Show("No history available.", "History");
                return;
            }

            string historyText = string.Join("\n", history);
            MessageBox.Show(historyText, "Calculation History");
        }

        public void Percent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(_display.Text);
                number /= 100;
                _display.Text = number.ToString();
            }
            catch (Exception)
            {
                _display.Text = "Error";
            }
        }

        public void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            _display.Text = "0";
        }

        public void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_display.Text) && _display.Text != "0")
            {
                _display.Text = _display.Text.Length > 1 ? _display.Text[..^1] : "0";
            }
        }

        public void Reciprocal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(_display.Text);
                if (number != 0)
                    _display.Text = (1 / number).ToString();
                else
                    _display.Text = "Error";
            }
            catch
            {
                _display.Text = "Error";
            }
        }

        public void Square_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(_display.Text);
                double result = number * number;

                _display.Text = result.ToString("N0", CultureInfo.CurrentCulture);
            }
            catch
            {
                _display.Text = "Error";
            }
        }

        public void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(_display.Text);
                if (number >= 0)
                {
                    double result = Math.Sqrt(number);

                    string formattedResult;
                    if (result % 1 == 0)
                    {
                        formattedResult = result.ToString("N0", CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        formattedResult = result.ToString("N2", CultureInfo.CurrentCulture);
                    }

                    _display.Text = formattedResult;
                }
                else
                {
                    _display.Text = "Error";
                }
            }
            catch
            {
                _display.Text = "Error";
            }
        }

        public void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(_display.Text);
                _display.Text = (-number).ToString("N");
            }
            catch
            {
                _display.Text = "Error";
            }
        }

        public void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (!_display.Text.Contains("."))
            {
                _display.Text += ".";
            }
        }
    }
}
