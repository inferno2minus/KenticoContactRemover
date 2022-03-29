using I2M.KenticoContactRemover.Interfaces;
using I2M.KenticoContactRemover.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace I2M.KenticoContactRemover.Services
{
    public class ContactService : IContactService
    {
        private readonly ClientOptions _options;
        private readonly ILogger<ContactService> _logger;

        public ContactService(IOptions<ClientOptions> options, ILogger<ContactService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public List<int> GetAllContacts()
        {
            var result = new List<int>();

            try
            {
                var condition = string.Format(_options.InactiveQuery, DateTime.Now.AddDays(-_options.InactivePeriod));

                using var connection = new SqlConnection(_options.ConnectionString);
                using var command = new SqlCommand(condition, connection);

                connection.Open();

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var contactId = reader.GetInt32(reader.GetOrdinal("ContactID"));

                    result.Add(contactId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }

            return result;
        }

        public void DeleteContacts(IEnumerable<int> contactIds)
        {
            try
            {
                using var connection = new SqlConnection(_options.ConnectionString);
                using var command = new SqlCommand("Proc_OM_Contact_MassDelete", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandTimeout = 0;
                command.Parameters.Add("@where", SqlDbType.NVarChar).Value = BuildCondition(contactIds);
                command.Parameters.Add("@batchLimit", SqlDbType.Int).Value = _options.BatchLimit;

                connection.Open();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error: {ex.Message}");
            }
        }

        private static string BuildCondition(IEnumerable<int> contactIds)
        {
            return $"ContactID in ({string.Join(",", contactIds)})";
        }
    }
}
