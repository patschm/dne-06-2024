using InfraStructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ICounter _counter;

        public IndexModel(ICounter cnt, ILogger<IndexModel> logger)
        {
            _logger = logger;
            _counter = cnt;
        }

        public void OnGet()
        {
            _counter.Increment();
            _counter.Show();
        }
    }
}