using System.Threading.Tasks;
using AzureExperiments.StorageQueues.Messaging;
using Microsoft.AspNetCore.Mvc;

namespace AzureExperiments.StorageQueues.Controllers
{
    public class QueueController : Controller
    {
        private readonly IReceiveEndpoint receiveEndpoint;
        
        public QueueController(IReceiveEndpoint receiveEndpoint)
        {
            this.receiveEndpoint = receiveEndpoint;
        }
        
        public IActionResult Index()
        {
            return View(new QueueViewModel {
                IsStarted = receiveEndpoint.IsStarted
            });
        }

        public async Task<IActionResult> Start()
        {
            await receiveEndpoint.StartAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Stop()
        {
            await receiveEndpoint.StopAsync();
            return RedirectToAction(nameof(Index));
        }
    }

    public class QueueViewModel
    {
        public bool IsStarted { get; set; }
    }
}