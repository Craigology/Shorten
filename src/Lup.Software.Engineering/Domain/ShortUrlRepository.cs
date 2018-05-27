using System.Linq;
using System.Threading.Tasks;
using Lup.Software.Engineering.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lup.Software.Engineering.Domain
{
    public class ShortUrlRepository
    {
        private const string ByShortUrlTableName = "ByShortUrl";
        private const string ByOriginalUrlTableName = "ByOriginalUrl";

        private readonly TableStorageContext _tableStorageContext;
        private readonly string _partitionKey = $"v{ShortUrl.Version.ToString(3)}";

        public ShortUrlRepository(TableStorageContext tableStorageContext)
        {
            _tableStorageContext = tableStorageContext;
        }

        public async Task<ShortUrl> GetByShortUrl(string shortUrl)
        {
            var table = await _tableStorageContext.GetTableAsync(ByShortUrlTableName, true);

            var tableQuery = new TableQuery<ShortUrl>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, MakeSafeRowKey(shortUrl))
                )
            );

            var tableResult = await table.ExecuteQuerySegmentedAsync(tableQuery, new TableContinuationToken());

            return tableResult?.Results?.SingleOrDefault();
        }

        public async Task<ShortUrl> GetByOriginalUrl(string originalUrl)
        {
            var table = await _tableStorageContext.GetTableAsync(ByOriginalUrlTableName, true);

            var tableQuery = new TableQuery<ShortUrl>().Where(
                TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, _partitionKey),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, MakeSafeRowKey(originalUrl))
                )
            );

            var tableResult = await table.ExecuteQuerySegmentedAsync(tableQuery, new TableContinuationToken());

            return tableResult?.Results?.SingleOrDefault();
        }

        public async Task Save(ShortUrl shortUrl)
        {
            var byOriginalUrlTable = await _tableStorageContext.GetTableAsync(ByOriginalUrlTableName, true);
            await Save(byOriginalUrlTable, shortUrl, MakeSafeRowKey(shortUrl.Original));

            var byShortUrlTableName = await _tableStorageContext.GetTableAsync(ByShortUrlTableName, true);
            await Save(byShortUrlTableName, shortUrl, MakeSafeRowKey(shortUrl.Short));
        }

        private async Task Save(CloudTable table, ShortUrl shortUrl, string rowKey)
        {
            shortUrl.PartitionKey = _partitionKey;
            shortUrl.RowKey = rowKey;
            shortUrl.ETag = "*";

            var mergeOperation = TableOperation.InsertOrMerge(shortUrl);
            await table.ExecuteAsync(mergeOperation);
        }

        private string MakeSafeRowKey(string candidateRowKey)
        {
            // https://docs.microsoft.com/en-us/rest/api/storageservices/understanding-the-table-service-data-model#characters-disallowed-in-key-fields
            return candidateRowKey.Replace("/", "").Replace(":", "").Replace("?", "").Replace("#", "");
        }
    }
}
