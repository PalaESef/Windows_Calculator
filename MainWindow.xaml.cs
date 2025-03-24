using System.Buffers;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Windows_Calculator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private double firstNumber = 0;
    private string operation = "";
    private bool isNewEntry = true;
    private List<string> history = new List<string>();
    private List<double> memoryStack = new List<double>();
    private double memoryValue = 0;
    private bool useThousandSeparator = false;
    private MemoryManager memoryManager = new MemoryManager();
    private KeyboardInputManager _keyboardInputManager;
    private ClipboardHandler clipboardHandler;
    private Calculator _calculator;

    public MainWindow()
    {
        InitializeComponent();
        _calculator = new Calculator(Display);
        _keyboardInputManager = new KeyboardInputManager(_calculator);

        this.PreviewKeyDown += Window_PreviewKeyDown;
        this.KeyDown += _keyboardInputManager.HandleKeyPress;

        clipboardHandler = new ClipboardHandler(Display);
        CutMenuItem.Click += clipboardHandler.HandleCut;
        CopyMenuItem.Click += clipboardHandler.HandleCopy;
        PasteMenuItem.Click += clipboardHandler.HandlePaste;
        AboutMenuItem.Click += clipboardHandler.HandleAbout;
        HistoryButton.Click += History_Click;
    }

    private void History_Click(object sender, RoutedEventArgs e)
    {
        _calculator.History_Click(sender, e);
    }
    private void Number_Click(object sender, RoutedEventArgs e) => _calculator.Number_Click(sender, e);
    private void Operator_Click(object sender, RoutedEventArgs e) => _calculator.Operator_Click(sender, e);
    private void Equals_Click(object sender, RoutedEventArgs e) => _calculator.Equals_Click(sender, e);
    private void Clear_Click(object sender, RoutedEventArgs e) => _calculator.Clear_Click(sender, e);
    private void ClearEntry_Click(object sender, RoutedEventArgs e) => _calculator.ClearEntry_Click(sender, e);
    private void Backspace_Click(object sender, RoutedEventArgs e) => _calculator.Backspace_Click(sender, e);
    private void Percent_Click(object sender, RoutedEventArgs e) => _calculator.Percent_Click(sender, e);
    private void Reciprocal_Click(object sender, RoutedEventArgs e) => _calculator.Reciprocal_Click(sender, e);
    private void Square_Click(object sender, RoutedEventArgs e) => _calculator.Square_Click(sender, e);
    private void SquareRoot_Click(object sender, RoutedEventArgs e) => _calculator.SquareRoot_Click(sender, e);
    private void ToggleSign_Click(object sender, RoutedEventArgs e) => _calculator.ToggleSign_Click(sender, e);
    private void Decimal_Click(object sender, RoutedEventArgs e) => _calculator.Decimal_Click(sender, e);

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

    private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        _keyboardInputManager.HandleKeyPress(sender, e);
    }
}

