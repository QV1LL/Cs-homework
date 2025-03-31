using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Task2.Services;

namespace Task2;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        this.CurrentNumberTextBlock.Text = "";
        this.CurrentExpressionTextBlock.Text = "";
    }

    private void DigitOrDotButton_Click(object sender, RoutedEventArgs e)
        => this.CurrentNumberTextBlock.Text += (sender as Button).Content;

    private void OperationButton_Click(object sender, RoutedEventArgs e)
    {
        this.CurrentExpressionTextBlock.Text += 
            this.CurrentNumberTextBlock.Text + (sender as Button).Content;
        this.CurrentNumberTextBlock.Text = "";
    }

    private void CButton_Click(object sender, RoutedEventArgs e)
    {
        this.CurrentExpressionTextBlock.Text = "";
        this.CurrentNumberTextBlock.Text = "";
    }

    private void CEButton_Click(object sender, RoutedEventArgs e)
        => this.CurrentNumberTextBlock.Text = "";

    private void BackspaceButton_Click(object sender, RoutedEventArgs e)
    {
        if (this.CurrentNumberTextBlock.Text.Length > 0)
        {
            int length = this.CurrentNumberTextBlock.Text.Length;
            this.CurrentNumberTextBlock.Text = new string(this.CurrentNumberTextBlock.Text.Take(length - 1).ToArray());
        }
    }

    private void EqualsButton_Click(object sender, RoutedEventArgs e)
    {
        this.CurrentExpressionTextBlock.Text += this.CurrentNumberTextBlock.Text;

        string result = ExpressionEvaluator.Evaluate(this.CurrentExpressionTextBlock.Text);
        this.CurrentExpressionTextBlock.Text = "";

        this.CurrentNumberTextBlock.Text = result;
    }
}