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
            var stopwatch = new Stopwatch();

            stopwatch.Start();

            _logger.LogInformation("Start cleaning contacts");
            _logger.LogInformation($"Config: BatchLimit: {_options.BatchLimit}, InactivePeriod: {_options.InactivePeriod}");

            var processedCount = 0;

            var contacts = _contactService.GetAllContacts();

            foreach (var batchContacts in contacts.Chunk(_options.BatchLimit))
            {
                var batches = batchContacts.ToList();

                processedCount += batches.Count;

                _contactService.DeleteContacts(batches);

                var elapsed = stopwatch.Elapsed;

                _logger.LogInformation($"Processed: {processedCount} | {contacts.Count - processedCount} | " +
                                       $"Elapsed: {elapsed.Hours}h {elapsed.Minutes}m {elapsed.Seconds}s | " +
                                       $"{(int)(processedCount / (double)contacts.Count * 100.0)}%");
            }

            _logger.LogInformation("Done");
        }
    }
}
