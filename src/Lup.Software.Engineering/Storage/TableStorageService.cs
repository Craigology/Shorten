using System;
using System.Threading.Tasks;
using Lup.Software.Engineering.Models;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lup.Software.Engineering.Storage
{
    public class TableStorageContext
    {
        private readonly CloudTableClient _tableClient;

        public TableStorageContext(IOptions<AppSettings> appSettings)
        {
            var storageAccount = CloudStorageAccount.Parse(appSettings.Value.ConnectionStrings.TableStorage);
            _tableClient = storageAccount.CreateCloudTableClient();
        }

        public async Task<CloudTable> GetTableAsync(string tableName, bool allowCreate)
        {
            var table = _tableClient.GetTableReference(tableName);

            if (!allowCreate && await table.ExistsAsync() == false)
                throw new ApplicationException($"Attempt to access table for reading: {tableName} that does not exist.");

            await table.CreateIfNotExistsAsync();

            return table;
        }
    }
}
