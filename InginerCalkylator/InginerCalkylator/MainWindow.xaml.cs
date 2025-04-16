using System;
using System.Windows;
using System.Windows.Controls;

namespace InginerCalkylator
{
    public partial class MainWindow : Window
    {
        private string currentInput = string.Empty;
        private string previousInput = string.Empty;
        private char operation;
        private bool newInput = true;
        private bool operationPending = false;
        private double memoryValue = 0;

        public MainWindow()
        {
            InitializeComponent();
            ClearAll();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            string buttonContent = button.Content.ToString();

            switch (buttonContent)
            {
                case "0":
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                case "6":
                case "7":
                case "8":
                case "9":
                    if (newInput)
                    {
                        currentInput = buttonContent;
                        newInput = false;
                    }
                    else
                    {
                        currentInput += buttonContent;
                    }
                    UpdateDisplay();
                    break;

                case ",":
                    if (newInput)
                    {
                        currentInput = "0,";
                        newInput = false;
                    }
                    else if (!currentInput.Contains(","))
                    {
                        currentInput += ",";
                    }
                    UpdateDisplay();
                    break;

                case "+":
                case "-":
                case "×":
                case "÷":
                    if (!operationPending)
                    {
                        previousInput = currentInput;
                        operation = buttonContent[0];
                        newInput = true;
                        operationPending = true;
                    }
                    else
                    {
                        Calculate();
                        operation = buttonContent[0];
                    }
                    UpdateDisplay();
                    break;

                case "=":
                    Calculate();
                    operationPending = false;
                    UpdateDisplay();
                    break;

                case "CE":
                    ClearEntry();
                    break;

                case "⌫":
                    if (currentInput.Length > 0)
                    {
                        currentInput = currentInput.Substring(0, currentInput.Length - 1);
                        if (currentInput.Length == 0)
                        {
                            currentInput = "0";
                            newInput = true;
                        }
                        UpdateDisplay();
                    }
                    break;

                case "+/-":
                    if (currentInput.StartsWith("-"))
                    {
                        currentInput = currentInput.Substring(1);
                    }
                    else if (currentInput != "0")
                    {
                        currentInput = "-" + currentInput;
                    }
                    UpdateDisplay();
                    break;

                case "π":
                    currentInput = Math.PI.ToString();
                    newInput = true;
                    UpdateDisplay();
                    break;

                case "e":
                    currentInput = Math.E.ToString();
                    newInput = true;
                    UpdateDisplay();
                    break;

                case "sin":
                    CalculateTrigFunction(Math.Sin);
                    break;

                case "cos":
                    CalculateTrigFunction(Math.Cos);
                    break;

                case "tg":
                    CalculateTrigFunction(Math.Tan);
                    break;

                case "log":
                    if (TryParseInput(out double logValue))
                    {
                        currentInput = Math.Log10(logValue).ToString();
                        newInput = true;
                        UpdateDisplay();
                    }
                    break;

                case "ln":
                    if (TryParseInput(out double lnValue))
                    {
                        currentInput = Math.Log(lnValue).ToString();
                        newInput = true;
                        UpdateDisplay();
                    }
                    break;

                case "x^2":
                    if (TryParseInput(out double squareValue))
                    {
                        currentInput = (squareValue * squareValue).ToString();
                        newInput = true;
                        UpdateDisplay();
                    }
                    break;

                case "x^y":
                    if (!operationPending)
                    {
                        previousInput = currentInput;
                        operation = '^';
                        newInput = true;
                        operationPending = true;
                    }
                    break;

                case "10^x":
                    if (TryParseInput(out double tenPowerValue))
                    {
                        currentInput = Math.Pow(10, tenPowerValue).ToString();
                        newInput = true;
                        UpdateDisplay();
                    }
                    break;

                case "√x":
                    if (TryParseInput(out double sqrtValue) && sqrtValue >= 0)
                    {
                        currentInput = Math.Sqrt(sqrtValue).ToString();
                        newInput = true;
                        UpdateDisplay();
                    }
                    break;

                case "1/x":
                    if (TryParseInput(out double reciprocalValue) && reciprocalValue != 0)
                    {
                        currentInput = (1 / reciprocalValue).ToString();
                        newInput = true;
                        UpdateDisplay();
                    }
                    break;

                case "n!":
                    if (TryParseInput(out double factorialValue) && factorialValue >= 0 && factorialValue <= 170)
                    {
                        currentInput = Factorial(factorialValue).ToString();
                        newInput = true;
                        UpdateDisplay();
                    }
                    break;

                case "|x|":
                    if (TryParseInput(out double absValue))
                    {
                        currentInput = Math.Abs(absValue).ToString();
                        newInput = true;
                        UpdateDisplay();
                    }
                    break;
            }
        }

        private void Calculate()
        {
            if (!operationPending || !double.TryParse(previousInput, out double first) || !double.TryParse(currentInput, out double second))
                return;

            double result = 0;
            switch (operation)
            {
                case '+':
                    result = first + second;
                    break;
                case '-':
                    result = first - second;
                    break;
                case '×':
                    result = first * second;
                    break;
                case '÷':
                    if (second != 0)
                        result = first / second;
                    else
                        result = 0;
                    break;
                case '^':
                    result = Math.Pow(first, second);
                    break;
            }

            currentInput = result.ToString();
            previousInput = string.Empty;
            newInput = true;
            operationPending = false;
        }

        private void CalculateTrigFunction(Func<double, double> trigFunction)
        {
            if (TryParseInput(out double value))
            {
                double radians = value * Math.PI / 180;
                currentInput = trigFunction(radians).ToString();
                newInput = true;
                UpdateDisplay();
            }
        }

        private double Factorial(double n)
        {
            double result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }

        private bool TryParseInput(out double value)
        {
            return double.TryParse(currentInput.Replace(",", "."), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out value);
        }

        private void ClearAll()
        {
            currentInput = "0";
            previousInput = string.Empty;
            operation = '\0';
            newInput = true;
            operationPending = false;
            UpdateDisplay();
        }

        private void ClearEntry()
        {
            currentInput = "0";
            newInput = true;
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            DownBlock.Text = currentInput;
            UpBlock.Text = operationPending ? $"{previousInput} {operation}" : string.Empty;
        }
    }
}