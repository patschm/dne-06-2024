namespace Calculator;

public partial class CalculatorApp : Form
{
    private SynchronizationContext? _main;
    public CalculatorApp()
    {
        InitializeComponent();
        _main = SynchronizationContext.Current;
    }

    private async void button1_Click(object sender, EventArgs e)
    {
        if (int.TryParse(txtA.Text, out int a) && int.TryParse(txtB.Text, out int b)) 
        {
            //var t1 = new Task<int>(() =>LongAdd(a, b));
            //t1.ContinueWith(t => UpdateAnswer(t.Result));
            //t1.Start();

            // Of korter
            //Task.Run(()=>LongAdd(a,b))
            //    .ContinueWith(pt=> {
            //        _main?.Post(UpdateAnswer, pt.Result);
            //        //UpdateAnswer(pt.Result)
            //        });
            int result = DoeIets(a, b).Result; // Dead lock
            UpdateAnswer(result);
        }      
    }

    private async Task<int> DoeIets(int a, int b)
    {
        int result = await LongAddAsync(a, b);//.ConfigureAwait(false);
        return result;
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
        return Task.Run<int>(() => LongAdd(a, b));
    }

}