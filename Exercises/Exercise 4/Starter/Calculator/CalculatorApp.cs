namespace Calculator;

public partial class CalculatorApp : Form
{
    public CalculatorApp()
    {
        InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
        if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b)) 
        {
            var result = LongAdd(a, b);
            UpdateAnswer(result);
        }      
    }

    private void UpdateAnswer(object? result)
    {
        lblAnswer.Text = result?.ToString();
    }

    private int LongAdd(int a, int b)
    {
        Task.Delay(10000).Wait();
        return a + b;
    }
}