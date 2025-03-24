using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Microsoft.VisualBasic;

namespace Windows_Calculator
{
    public class MemoryManager
    {
        private List<double> memoryStack = new List<double>();

        public void MemoryClear_Click(object sender, RoutedEventArgs e, TextBox Display)
        {
            if (memoryStack.Count > 0)
            {
                string action = ShowClearMemoryDialog();

                if (action == "Clear All")
                {
                    memoryStack.Clear();
                    Display.Text = "0";
                    MessageBox.Show("All memory entries cleared.", "Memory");
                }
                else if (action == "Select Entry")
                {
                    string selectedMemory = ShowMemorySelectionDialog("Clear Memory");

                    if (!string.IsNullOrEmpty(selectedMemory))
                    {
                        double selectedValue = double.Parse(selectedMemory);
                        memoryStack.Remove(selectedValue);
                        MessageBox.Show("Memory entry removed.", "Memory");
                    }
                }
                else
                {
                    MessageBox.Show("No valid action selected.", "Error");
                }
            }
            else
            {
                MessageBox.Show("No memory available.", "Error");
            }
        }

        public void MemoryRecall_Click(object sender, RoutedEventArgs e, TextBox Display)
        {
            Display.Text = memoryStack.Count > 0 ? memoryStack[^1].ToString() : "0";
        }

        public void MemoryAdd_Click(object sender, RoutedEventArgs e, TextBox Display)
        {
            if (memoryStack.Count > 0)
            {
                string selectedMemory = ShowMemorySelectionDialog("Add to Memory");

                if (!string.IsNullOrEmpty(selectedMemory))
                {
                    double selectedValue = double.Parse(selectedMemory);
                    try
                    {
                        double number = double.Parse(Display.Text);
                        int selectedIndex = memoryStack.IndexOf(selectedValue);
                        memoryStack[selectedIndex] += number;
                    }
                    catch
                    {
                        Display.Text = "Error";
                    }
                }
            }
            else
            {
                MessageBox.Show("No memory available.", "Error");
            }
        }

        public void MemorySubtract_Click(object sender, RoutedEventArgs e, TextBox Display)
        {
            if (memoryStack.Count > 0)
            {
                string selectedMemory = ShowMemorySelectionDialog("Subtract from Memory");

                if (!string.IsNullOrEmpty(selectedMemory))
                {
                    double selectedValue = double.Parse(selectedMemory);
                    try
                    {
                        double number = double.Parse(Display.Text);
                        int selectedIndex = memoryStack.IndexOf(selectedValue);
                        memoryStack[selectedIndex] -= number;
                    }
                    catch
                    {
                        Display.Text = "Error";
                    }
                }
            }
            else
            {
                MessageBox.Show("No memory available.", "Error");
            }
        }

        public void MemoryStore_Click(object sender, RoutedEventArgs e, TextBox Display)
        {
            try
            {
                double number = double.Parse(Display.Text);
                memoryStack.Add(number);
            }
            catch
            {
                Display.Text = "Error";
            }
        }

        public void MemoryView_Click(object sender, RoutedEventArgs e)
        {
            if (memoryStack.Count == 0)
            {
                MessageBox.Show("No stored values.", "Memory");
                return;
            }

            string memoryText = string.Join("\n", memoryStack.AsEnumerable().Reverse());
            MessageBox.Show(memoryText, "Memory Values");
        }

        private string ShowClearMemoryDialog()
        {
            string[] options = { "Clear All", "Select Entry" };
            string selectedOption = Interaction.InputBox(
                "Choose an option:\n1. Clear All\n2. Select Entry",
                "Clear Memory",
                "Select Entry");

            return options.Contains(selectedOption) ? selectedOption : string.Empty;
        }

        private string ShowMemorySelectionDialog(string operation)
        {
            if (memoryStack.Count == 0)
            {
                MessageBox.Show("No memory available.", "Error");
                return null;
            }

            string memoryText = string.Join("\n", memoryStack.Select((value, index) => $"{index + 1}. {value}"));

            string input = Interaction.InputBox(
                $"Select the memory entry to {operation}:\n{memoryText}",
                operation,
                "1");

            return int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= memoryStack.Count
                ? memoryStack[selectedIndex - 1].ToString()
                : null;
        }
    }
}
