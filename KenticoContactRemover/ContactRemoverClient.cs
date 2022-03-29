using I2M.KenticoContactRemover.Interfaces;
using I2M.KenticoContactRemover.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Linq;

namespace I2M.KenticoContactRemover
{
    public class ContactRemoverClient : IContactRemoverClient
    {
        private readonly ClientOptions _options;
        private readonly IContactService _contactService;
        private readonly ILogger<ContactRemoverClient> _logger;

        public ContactRemoverClient(IOptions<ClientOptions> options, IContactService contactService, ILogger<ContactRemoverClient> logger)
        {
            _options = options.Value;
            _contactService = contactService;
            _logger = logger;
        }

        public void Execute()
        {
            var watch = new Stopwatch();

            watch.Start();

            _logger.LogInformation("Start cleaning");
            _logger.LogInformation($"Config: BatchLimit: {_options.BatchLimit}, InactivePeriod: {_options.InactivePeriod}");

            var processedCount = 0;

            var contactIds = _contactService.GetAllContacts();

            foreach (var batchContactIds in contactIds.Chunk(_options.BatchLimit))
            {
                var batches = batchContactIds.ToList();

                processedCount += batches.Count;

                _contactService.DeleteContacts(batches);

                var timeSpan = watch.Elapsed;

                _logger.LogInformation($"Processed: {processedCount} | {contactIds.Count - processedCount} | " +
                                       $"Elapsed: {timeSpan.Hours}h {timeSpan.Minutes}m {timeSpan.Seconds}s | " +
                                       $"{(int)(processedCount / (double)contactIds.Count * 100.0)}%");
            }

            _logger.LogInformation("Done");
        }
    }
}
