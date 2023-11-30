using AsproRDTool.ServiceContracts.Models;
using Microsoft.SqlServer.Management.Smo;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace AsproRDTool.Data
{
    public class DBService
    {
        public DataSet GetTableData(JsonBreakerDetail detail, BackgroundWorker worker)
        {
            DataSet chartDraft = new DataSet();
            int totalRowCount = 0;
            int rowCount = 0;
            int progressPercentage = 0;
            try
            {
                string connectionString = $"Data Source={detail.server};Integrated Security = sspi;Persist Security Info=True;Initial Catalog={detail.dbName};";
                string query = "";
                foreach (var item in detail.tableList)
                {
                    query = $"select Count(*) from {item}";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            totalRowCount += (int)cmd.ExecuteScalar();
                        }
                    }
                }
                foreach (var tableName in detail.tableList)
                {

                    query = $"select Count(*) from {tableName}";
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        using (SqlCommand cmd = new SqlCommand(query, con))
                        {
                            rowCount = (int)cmd.ExecuteScalar();
                        }
                    }
                    for (int i = 0; i <= rowCount; i += 10000)
                    {

                        using (SqlConnection con = new SqlConnection(connectionString))
                        {
                            switch (tableName)
                            {
                                case "tooth_chart_history":
                                    query = $"SELECT TOP 10000 * FROM(SELECT id,tooth_chart FROM tooth_chart_history ORDER BY id OFFSET {i} ROWS)A";
                                    break;
                                case "perio_history":
                                    query = $"SELECT TOP 10000 * FROM(SELECT id,gum_measurement FROM perio_history ORDER BY id OFFSET {i} ROWS)A";
                                    break;
                            }
                            using (SqlCommand cmd = new SqlCommand(query, con))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                                {
                                    if (!chartDraft.Tables.Contains(tableName))
                                    {
                                        chartDraft.Tables.Add(tableName);
                                    }
                                    da.Fill(chartDraft.Tables[tableName]);
                                }
                            }
                        }
                        worker.ReportProgress(GetDataFetchProgress(totalRowCount, i, progressPercentage));
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return chartDraft;
        }
        private int GetDataFetchProgress(int totalRowCount, int currentRowCount, int progressPercentage)
        {
            if (progressPercentage <= currentRowCount * 100 / totalRowCount)
            {
                progressPercentage = currentRowCount * 100 / totalRowCount;
            }

            return progressPercentage;
        }
        public void CreateJsonTables(JsonBreakerDetail detail, DataSet data, BackgroundWorker worker)
        {
            int tableCount = 0;
            string connectionString = $"Data Source={detail.server};Integrated Security = sspi;Persist Security Info=True;Initial Catalog={detail.dbName};";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                Server server = new Server();
                server.ConnectionContext.LoginSecure = true;
                server.ConnectionContext.ServerInstance = detail.server;
                Database db = new Database();
                db = server.Databases[detail.dbName];
                foreach (DataTable dt in data.Tables)
                {
                    tableCount++;
                    Table table = new Table(db, dt.TableName);
                    foreach (DataColumn dc in dt.Columns)
                    {
                        Column col = new Column(table, dc.ColumnName);
                        col.DataType = GetDataType(dc.DataType.ToString(), dc.MaxLength);
                        table.Columns.Add(col);
                    }
                    if (db.Tables.Contains(dt.TableName))
                    {
                        db.Tables[dt.TableName].DropIfExists();
                    }
                    table.Create();
                }
                worker.ReportProgress(tableCount * 100 / data.Tables.Count);
            }

        }
        public DataType GetDataType(string datatype, int datalength)
        {
            DataType DTTemp = null;

            switch (datatype)
            {
                case "System.Decimal":
                    DTTemp = DataType.Float;
                    break;
                case "System.String":
                    DTTemp = DataType.VarChar(datalength);
                    break;
                case "System.Int32":
                    DTTemp = DataType.Int;
                    break;
                case "System.Boolean":
                    DTTemp = DataType.Bit;
                    break;
                case "System.Single":
                    DTTemp = DataType.Float;
                    break;
                case "System.DateTime":
                    DTTemp = DataType.DateTime;
                    break;
            }
            return DTTemp;
        }
        public void DataSetBulkCopy(JsonBreakerDetail detail, DataSet data, BackgroundWorker worker)
        {
            int tableCount = 0;
            string connectionString = $"Data Source={detail.server} ;Integrated Security = sspi;Persist Security Info=True;Initial Catalog= {detail.dbName};";
            using (SqlConnection con = new SqlConnection(connectionString))
            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(con))
            {
                con.Open();
                bulkCopy.BatchSize = 5000;
                bulkCopy.BulkCopyTimeout = 60;
                foreach (DataTable table in data.Tables)
                {
                    tableCount++;
                    bulkCopy.DestinationTableName = $"[{table.TableName}]";
                    bulkCopy.WriteToServer(table);
                    worker.ReportProgress(tableCount * 100 / data.Tables.Count);
                }
            }
        }

        public async Task<List<string>> GetDBDetails(string server)
        {
            List<string> dbList = new List<string>();
            string connectionString = $"Data Source={server};Integrated Security = sspi;Initial Catalog= master";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                DataTable dt = con.GetSchema("Databases");
                foreach (DataRow row in dt.Rows)
                {
                    dbList.Add(row[0].ToString());
                }
            }
            return dbList;
        }
    }
}