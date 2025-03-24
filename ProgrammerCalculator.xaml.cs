using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Windows_Calculator
{
    /// <summary>
    /// Interaction logic for ProgrammerCalculator.xaml
    /// </summary>
    public partial class ProgrammerCalculator : Window
    {
        private string selectedSystem = "DEC";
        private double firstNumber = 0;
        private string operation = "";
        private bool isNewEntry = true;
        private List<string> history = new List<string>();
        private List<double> memoryStack = new List<double>();
        private double memoryValue = 0;
        private MemoryManager memoryManager = new MemoryManager();
        public ProgrammerCalculator()
        {
            InitializeComponent();
            UpdateButtonStates();
            HistoryButton.Click += History_Click;
        }


        private void MemoryClear_Click(object sender, RoutedEventArgs e) => memoryManager.MemoryClear_Click(sender, e, Display);
        private void MemoryRecall_Click(object sender, RoutedEventArgs e) => memoryManager.MemoryRecall_Click(sender, e, Display);
        private void MemoryAdd_Click(object sender, RoutedEventArgs e) => memoryManager.MemoryAdd_Click(sender, e, Display);
        private void MemorySubtract_Click(object sender, RoutedEventArgs e) => memoryManager.MemorySubtract_Click(sender, e, Display);
        private void MemoryStore_Click(object sender, RoutedEventArgs e) => memoryManager.MemoryStore_Click(sender, e, Display);
        private void MemoryView_Click(object sender, RoutedEventArgs e) => memoryManager.MemoryView_Click(sender, e);

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;
            else
                this.WindowState = WindowState.Normal;
        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private void UpdateConversionDisplay(double value)
        {
            int intValue = (int)value;

            if (HexValue != null) HexValue.Text = intValue.ToString("X");
            if (DecValue != null) DecValue.Text = intValue.ToString();
            if (OctValue != null) OctValue.Text = Convert.ToString(intValue, 8);
            if (BinValue != null) BinValue.Text = Convert.ToString(intValue, 2);
        }


        private void Equals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double secondNumber = ParseInput(Display.Text);
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
                            Display.Text = "Error";
                            return;
                        }
                        break;
                }

                string historyEntry = $"{firstNumber} {operation} {secondNumber} = {result}";
                history.Add(historyEntry);

                Display.Text = FormatOutput(result);
                isNewEntry = true;

                UpdateConversionDisplay(result);
            }
            catch
            {
                Display.Text = "Error";
            }
        }

        private void Operator_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                try
                {
                    firstNumber = ParseInput(Display.Text);
                    operation = button.Content.ToString();
                    Display.Text = "0";

                    UpdateConversionDisplay(firstNumber);
                }
                catch (Exception)
                {
                    Display.Text = "Error";
                }
            }
        }

        private double ParseInput(string input)
        {
            if (selectedSystem == "HEX")
                return Convert.ToInt32(input, 16);
            else if (selectedSystem == "OCT")
                return Convert.ToInt32(input, 8);
            else if (selectedSystem == "BIN")
                return Convert.ToInt32(input, 2);
            else // Decimal
                return double.Parse(input);
        }

        private string FormatOutput(double value)
        {
            int intValue = (int)value;

            if (selectedSystem == "HEX")
                return intValue.ToString("X");
            else if (selectedSystem == "OCT")
                return Convert.ToString(intValue, 8);
            else if (selectedSystem == "BIN")
                return Convert.ToString(intValue, 2);
            else
                return value.ToString();
        }


        private void Number_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (Display.Text == "0" || operation == "=")
                    Display.Text = button.Content.ToString();
                else
                    Display.Text += button.Content.ToString();

                UpdateConversionDisplay(ParseInput(Display.Text));
            }
        }



        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
            firstNumber = 0;
            operation = "";
            isNewEntry = true;
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            if (history.Count == 0)
            {
                MessageBox.Show("No history available.", "History");
                return;
            }

            string historyText = string.Join("\n", history);
            MessageBox.Show(historyText, "Calculation History");
        }

        private void Percent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(Display.Text);
                number /= 100; // Convert to percentage
                Display.Text = number.ToString();
            }
            catch (Exception)
            {
                Display.Text = "Error";
            }
        }

        private void ClearEntry_Click(object sender, RoutedEventArgs e)
        {
            Display.Text = "0";
        }

        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(Display.Text) && Display.Text != "0")
            {
                Display.Text = Display.Text.Length > 1 ? Display.Text[..^1] : "0";
            }
        }

        private void Reciprocal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(Display.Text);
                if (number != 0)
                    Display.Text = (1 / number).ToString();
                else
                    Display.Text = "Error";
            }
            catch
            {
                Display.Text = "Error";
            }
        }

        private void Square_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(Display.Text);
                Display.Text = (number * number).ToString();
            }
            catch
            {
                Display.Text = "Error";
            }
        }

        private void SquareRoot_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(Display.Text);
                if (number >= 0)
                    Display.Text = Math.Sqrt(number).ToString();
                else
                    Display.Text = "Error"; // Avoid square root of negative numbers
            }
            catch
            {
                Display.Text = "Error";
            }
        }

        private void ToggleSign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double number = double.Parse(Display.Text);
                Display.Text = (-number).ToString();
            }
            catch
            {
                Display.Text = "Error";
            }
        }

        private void Decimal_Click(object sender, RoutedEventArgs e)
        {
            if (!Display.Text.Contains("."))
            {
                Display.Text += ".";
            }
        }
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuPanel.Visibility == Visibility.Collapsed)
            {
                MenuPanel.Visibility = Visibility.Visible;
            }
            else
            {
                MenuPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void MenuList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuList.SelectedItem is ListViewItem selectedItem)
            {
                string menuText = ((selectedItem.Content as StackPanel)?.Children[1] as TextBlock)?.Text;

                if (!string.IsNullOrEmpty(menuText))
                {
                    SwitchCalculatorMode(menuText);
                }
                MenuList.SelectedItem = null;
            }
        }

        private void SwitchCalculatorMode(string mode)
        {
            Window newCalculator = null;

            switch (mode)
            {
                case "Standard":
                    newCalculator = new MainWindow();
                    break;
                case "Programmer":
                    newCalculator = new ProgrammerCalculator();
                    break;
                default:
                    MessageBox.Show("Unknown mode selected.");
                    return;
            }

            if (newCalculator != null)
            {
                newCalculator.Show();
                this.Hide();
            }
        }

        private void Parentheses_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button != null)
            {
                if (button.Content.ToString() == "(")
                {
                    Display.Text += "(";
                }
                else if (button.Content.ToString() == ")")
                {
                    Display.Text += ")";
                }
            }
        }

        private void ToggleMemoryDropdown(object sender, RoutedEventArgs e)
        {
            MemoryPopup.IsOpen = !MemoryPopup.IsOpen;
        }

        private void OnNumberSystemChanged(object sender, RoutedEventArgs e)
        {
            if (HexRadio.IsChecked == true)
                selectedSystem = "HEX";
            else if (DecRadio.IsChecked == true)
                selectedSystem = "DEC";
            else if (OctRadio.IsChecked == true)
                selectedSystem = "OCT";
            else if (BinRadio.IsChecked == true)
                selectedSystem = "BIN";

            UpdateCalculationDisplay();
            UpdateButtonStates();
        }

        private void UpdateButtonStates()
        {
            if (HexRadio == null || DecRadio == null || OctRadio == null || BinRadio == null)
                return;

            HexRadio.IsEnabled = selectedSystem != "HEX";
            DecRadio.IsEnabled = selectedSystem != "DEC";
            OctRadio.IsEnabled = selectedSystem != "OCT";
            BinRadio.IsEnabled = selectedSystem != "BIN";

            bool isHex = selectedSystem == "HEX";
            bool isDec = selectedSystem == "DEC";
            bool isOct = selectedSystem == "OCT";
            bool isBin = selectedSystem == "BIN";

            if (A_Button != null) A_Button.IsEnabled = isHex;
            if (B_Button != null) B_Button.IsEnabled = isHex;
            if (C_Button != null) C_Button.IsEnabled = isHex;
            if (D_Button != null) D_Button.IsEnabled = isHex;
            if (E_Button != null) E_Button.IsEnabled = isHex;
            if (F_Button != null) F_Button.IsEnabled = isHex;

            if (Num8_Button != null) Num8_Button.IsEnabled = isHex || isDec;
            if (Num9_Button != null) Num9_Button.IsEnabled = isHex || isDec;

            if (Num2_Button != null) Num2_Button.IsEnabled = !isBin;
            if (Num3_Button != null) Num3_Button.IsEnabled = !isBin;
            if (Num4_Button != null) Num4_Button.IsEnabled = !isBin;
            if (Num5_Button != null) Num5_Button.IsEnabled = !isBin;
            if (Num6_Button != null) Num6_Button.IsEnabled = !isBin;
            if (Num7_Button != null) Num7_Button.IsEnabled = !isBin;

            if (Decimal_Button != null) Decimal_Button.IsEnabled = isDec;
        }


        private void UpdateCalculationDisplay()
        {
            if (!int.TryParse(Display.Text, out int value))
            {
                Display.Text = "0";
                value = 0;
            }

            if (HexValue != null) HexValue.Text = value.ToString("X");
            if (DecValue != null) DecValue.Text = value.ToString();
            if (OctValue != null) OctValue.Text = Convert.ToString(value, 8);
            if (BinValue != null) BinValue.Text = Convert.ToString(value, 2);
        }
    }
}
