using Microsoft.Azure.Cosmos.Table;
using System;

namespace NewAzureTableStorage
{
    class Program
    {
        public class EmployeeEntity : TableEntity
        {
            public EmployeeEntity() { }
            public EmployeeEntity(string deptName, string empName)
            {
                PartitionKey = deptName;
                RowKey = empName;
            }

            public string Email { get; set; }
            public string PhoneNumber { get; set; }
            public string Specialization { get; set; }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Table storage sample");
            
            var storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=apithreelayerarchstorage;AccountKey=QERpGqXXMGMLOlG1zCHdJAeANoFpdSiTm7YRc+IhrDobHoD61S7HLZXx2SfXN/ASGoqskVjKLmIx5hzWFHLmAg==;EndpointSuffix=core.windows.net";           
            var tableName = "tsmtable";

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(); //(new TableClientConfiguration());
            CloudTable tableEmployee = tableClient.GetTableReference(tableName);
            tableEmployee.CreateIfNotExists();

            EmployeeEntity emp1 = new EmployeeEntity("Training1", "Emp1")
            {
                Email = "emp1@gmail.com",
                PhoneNumber = "+91-9607865228"
            };
            EmployeeEntity emp2 = new EmployeeEntity("Training1", "Emp2")
            {
                Email = "emp2@gmail.com",
                PhoneNumber = "+91-7687056228"
            };
            EmployeeEntity emp3 = new EmployeeEntity("Development", "Emp1")
            {
                Email = "emp3@gmail.com",
                PhoneNumber = "+91-7852385228",
                Specialization = "Engineering"
            };

            TableBatchOperation batchOperation = new TableBatchOperation();

            //insert
            /*batchOperation.InsertOrReplace(emp1);
            batchOperation.InsertOrReplace(emp2);*/

            //batchOperation.InsertOrReplace(emp3);   //all batch operation run toghethet for same partition key so this will give error

            /*tableEmployee.ExecuteBatch(batchOperation);*/

            /*batchOperation = new TableBatchOperation();
            batchOperation.InsertOrReplace(emp3);
            tableEmployee.ExecuteBatch(batchOperation);*/

            //read
            /*TableOperation operation = TableOperation.Retrieve<EmployeeEntity>("Development", "Emp1");
            TableResult result = tableEmployee.Execute(operation);
            EmployeeEntity employee = result.Result as EmployeeEntity;
            if (employee != null)
            {
                Console.WriteLine("Fetched \t{0}\t{1}\t{2}\t{3}",
                    employee.PartitionKey, employee.RowKey, employee.Email, employee.PhoneNumber);
            }*/

            //delete
            /*emp3.ETag = "*";
            batchOperation.Delete(emp3);
            tableEmployee.ExecuteBatch(batchOperation);*/

            TableOperation operation = TableOperation.Retrieve<EmployeeEntity>("Tarun", "Singh Mahar");
            TableResult result = tableEmployee.Execute(operation);
            EmployeeEntity employee = result.Result as EmployeeEntity;
            employee.ETag = "*";
            batchOperation.Delete(employee);
            tableEmployee.ExecuteBatch(batchOperation);

        }
    }
}
