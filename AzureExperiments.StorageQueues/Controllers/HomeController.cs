using System.Diagnostics;
using System.Threading.Tasks;
using AzureExperiments.StorageQueues.Models;
using AzureExperiments.StorageQueues.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace AzureExperiments.StorageQueues.Controllers
{
    public class HomeController : Controller
    {
        public IOptions<AzureStorageOptions> StorageOptions { get; }

        public HomeController(IOptions<AzureStorageOptions> storageOptions)
        {
            StorageOptions = storageOptions;
        }

        private async Task<CloudQueueClient> GetQueueClient()
        {
            var storageAccount = CloudStorageAccount.Parse(StorageOptions.Value.ConnectionString);

            // Create the queue client.
            var queueClient = storageAccount.CreateCloudQueueClient();

            // Retrieve a reference to a container.
            var queue = queueClient.GetQueueReference("myqueue");

            // Create the queue if it doesn't already exist
            await queue.CreateIfNotExistsAsync();

            return queueClient;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}