using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace AzureTableStorage
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Table storage sample");

            var storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=apithreelayerarchstorage;AccountKey=QERpGqXXMGMLOlG1zCHdJAeANoFpdSiTm7YRc+IhrDobHoD61S7HLZXx2SfXN/ASGoqskVjKLmIx5hzWFHLmAg==;EndpointSuffix=core.windows.net";
            var tableName = "tsmtable";

            CloudStorageAccount storageAccount;
            storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            CloudTable table = tableClient.GetTableReference(tableName);


            CustomerEntity customer = new CustomerEntity("Tarun", "Singh Mahar")
            {
                Email = "tarun@gmail.com",
                PhoneNumber = "+91-7777766666"
            };


            //CreateCustomer(table, customer).Wait();

            //ReadCustomer(table, "Tarun", "Singh Mahar").Wait();

            //UpdateCustomer(table, customer).Wait();

            DeleteCustomer(table, "Tarun", "Singh Mahar").Wait();

        }

        public static async Task CreateCustomer(CloudTable table, CustomerEntity customer)
        {
            TableOperation createOperation = TableOperation.Insert(customer);

            // Execute the operation.
            TableResult result = await table.ExecuteAsync(createOperation);
            CustomerEntity createdCustomer = result.Result as CustomerEntity;

            Console.WriteLine("Created customer.");
        }

        public static async Task ReadCustomer(CloudTable table, string firstName, string lastName)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(firstName, lastName);

            TableResult result = await table.ExecuteAsync(retrieveOperation);
            CustomerEntity customer = result.Result as CustomerEntity;

            if (customer != null)
            {
                Console.WriteLine("Fetched \t{0}\t{1}\t{2}\t{3}",
                    customer.PartitionKey, customer.RowKey, customer.Email, customer.PhoneNumber);
            }
        }

        public static async Task UpdateCustomer(CloudTable table, CustomerEntity customer)
        {
            TableOperation updateOperation = TableOperation.InsertOrMerge(customer);

            // Execute the operation.
            TableResult result = await table.ExecuteAsync(updateOperation);
            CustomerEntity updatedCustomer = result.Result as CustomerEntity;

            Console.WriteLine("Updated user.");
        }

        public static async Task DeleteCustomer(CloudTable table, string firstName, string lastName)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<CustomerEntity>(firstName, lastName);
            TableResult result = await table.ExecuteAsync(retrieveOperation);
            CustomerEntity customer = result.Result as CustomerEntity;

            TableBatchOperation batchOperation = new TableBatchOperation();
            customer.ETag = "*";
            batchOperation.Delete(customer);
            table.ExecuteBatch(batchOperation);

            Console.WriteLine("Deleted customer");
        }
    }

    public class CustomerEntity : TableEntity
    {
        public CustomerEntity() { }
        public CustomerEntity(string lastName, string firstName)
        {
            PartitionKey = lastName;
            RowKey = firstName;
        }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}

