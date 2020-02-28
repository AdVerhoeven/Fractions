using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
namespace Fractions
{ 
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Fraction input = new Fraction(0, 1);
        readonly Stack<Fraction> fractions = new Stack<Fraction>();

        static string numerator = string.Empty;
        static string denominator = string.Empty;

        public MainWindow()
        {
            InitializeComponent();

            Fraction quarter = new Fraction(1, 4);
            Fraction ans = quarter.Sqrt(110);
            ans.Simplify();
            BigInteger div = (ans.Numerator * 2) / ans.Denominator;
            BigInteger mod = (ans.Numerator * 2) % ans.Denominator;
            BigInteger diff = mod - BigInteger.Pow(2, 110);
            Debug.WriteLine($"div: {div}\tdiff: {diff}");
        }

        #region TextBoxEvents
        private void NumeratorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!BigInteger.TryParse(NumeratorTextBox.Text, out _))
            {
                if (NumeratorTextBox.Text != "-" && NumeratorTextBox.Text != string.Empty)
                {
                    int selectionStart = NumeratorTextBox.SelectionStart - 1;
                    NumeratorTextBox.Text = numerator;

                    selectionStart = (selectionStart < 0) ? 0 : selectionStart;

                    NumeratorTextBox.Select(selectionStart, 0);
                }
                else
                {
                    numerator = "-";
                }
            }
            else
            {
                numerator = NumeratorTextBox.Text;
            }
        }

        private void DenominatorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!BigInteger.TryParse(DenominatorTextBox.Text, out _))
            {
                if (DenominatorTextBox.Text != string.Empty)
                {
                    int selectionStart = DenominatorTextBox.SelectionStart - 1;
                    DenominatorTextBox.Text = denominator;

                    selectionStart = (selectionStart < 0) ? 0 : selectionStart;
                    DenominatorTextBox.Select(selectionStart, 0);
                }
            }
            else
            {
                denominator = DenominatorTextBox.Text;
            }
        }
        #endregion

        #region ButtonEvents
        /// <summary>
        /// Displays the fraction obtained from the textboxes in the fraction label.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddFractionButton_Click(object sender, RoutedEventArgs e)
        {
            if (NumeratorTextBox.Text != string.Empty && DenominatorTextBox.Text != string.Empty)
            {
                BigInteger a = 0;
                if (BigInteger.TryParse(NumeratorTextBox.Text, out _))
                {
                    a = BigInteger.Parse(NumeratorTextBox.Text);
                    numerator = string.Empty;
                    NumeratorTextBox.Text = string.Empty;

                }
                BigInteger b = BigInteger.Parse(DenominatorTextBox.Text);

                denominator = string.Empty;
                DenominatorTextBox.Text = string.Empty;

                input = new Fraction(a, b);

                fractions.Push(input);
                FractionLabel.Items.Add(input);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (fractions.Count > 1)
            {
                Fraction first = fractions.Pop();
                Fraction second = fractions.Pop();
                FractionLabel.Items.Remove(first);
                FractionLabel.Items.Remove(second);
                fractions.Push(second + first);
                FractionLabel.Items.Add(fractions.Peek());
            }
        }

        private void SubtractButton_Click(object sender, RoutedEventArgs e)
        {
            if (fractions.Count > 1)
            {
                Fraction first = fractions.Pop();
                Fraction second = fractions.Pop();
                FractionLabel.Items.Remove(first);
                FractionLabel.Items.Remove(second);
                fractions.Push(second - first);
                FractionLabel.Items.Add(fractions.Peek());
            }
        }

        private void MultiplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (fractions.Count > 1)
            {
                Fraction first = fractions.Pop();
                Fraction second = fractions.Pop();
                FractionLabel.Items.Remove(first);
                FractionLabel.Items.Remove(second);
                fractions.Push(second * first);
                FractionLabel.Items.Add(fractions.Peek());
            }
        }

        private void DivideButton_Click(object sender, RoutedEventArgs e)
        {
            if (fractions.Count > 1)
            {
                Fraction first = fractions.Pop();
                if (first.Numerator != 0)
                {
                    Fraction second = fractions.Pop();
                    FractionLabel.Items.Remove(first);
                    FractionLabel.Items.Remove(second);
                    fractions.Push(second / first);
                    FractionLabel.Items.Add(fractions.Peek());
                }
                else
                {
                    fractions.Push(first);
                }
            }
        }

        private void SimplifyButton_Click(object sender, RoutedEventArgs e)
        {
            if (fractions.Count > 0)
            {
                FractionLabel.Items.Remove(fractions.Peek());
                fractions.Push(fractions.Pop().Simplify());
                FractionLabel.Items.Add(fractions.Peek());
            }
        }

        private void SquareButton_Click(object sender, RoutedEventArgs e)
        {
            if (fractions.Count > 0)
            {
                FractionLabel.Items.Remove(fractions.Peek());
                fractions.Push(fractions.Peek() * fractions.Pop());
                FractionLabel.Items.Add(fractions.Peek());
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            fractions.Clear();
            FractionLabel.Items.Clear();
        }
        #endregion

        /// <summary>
        /// Copies a fraction from the fractionlabel to the textbox so you can add it.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FractionLabel_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox.SelectedItem is Fraction fraction)
            {
                NumeratorTextBox.Text = fraction.Numerator.ToString();
                DenominatorTextBox.Text = fraction.Denominator.ToString();
            }
            else
            {
                NumeratorTextBox.Text = "0";
                DenominatorTextBox.Text = "1";
            }
        }

        #region helpermethods
        /// <summary>
        /// Returns the greatest common divisor of two integers.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static BigInteger GCD(BigInteger a, BigInteger b)
        {
            if (a == 0)
                return b;
            return GCD(b % a, a);
        }
        #endregion
    }
}
