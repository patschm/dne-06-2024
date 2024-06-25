namespace Calculator;

public partial class CalculatorApp : Form
{
    private SynchronizationContext? _main;
    public CalculatorApp()
    {
        _main = SynchronizationContext.Current;
        InitializeComponent();
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b)) 
        {
            //var result = LongAdd(a, b);
            //UpdateAnswer(result
            //Task.Run(() => LongAdd(a, b))
            //   .ContinueWith(pt=>_main?.Send(UpdateAnswer, pt.Result));
            //var result=await LongAddAsync(a, b);
            var result = DoeIets(a, b).Result; // Dead lock!
            UpdateAnswer(result);
        }
    }

    private async Task<int> DoeIets(int a, int b)
    {
        var res = await LongAddAsync(a,b).ConfigureAwait(false);
        return res;
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
    private Task<int> LongAddAsync(int a, int b)
    {
        return Task.Run(() => LongAdd(a, b));
    }
}