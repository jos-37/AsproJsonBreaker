using AsproRDTool.Data;
using AsproRDTool.ServiceContracts;
using AsproRDTool.ServiceContracts.Models;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Data;

namespace AsproRDTool.Services
{
    public class JsonBreakerBase : IJsonBreaker
    {
        public JsonBreakerStatus ProgressStatus { set; get; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
        private static Mutex mut = new Mutex();

        private int totalTableCount;
        private int tableCount = 0;
        private int totalJsonCount;
        private int rowindex = 1;
        protected CancellationToken cancellationToken;
        public virtual void InitiateJsonModification(JsonBreakerDetail detail, BackgroundWorker worker, DoWorkEventArgs e)
        {
            cancellationToken = CancellationTokenSource.Token;
            cancellationToken.ThrowIfCancellationRequested();
            DBService db = new DBService();
            ProgressStatus = new JsonBreakerStatus() { Status = "Fetching Json Tables...", progressPercentage = 0 };
            worker.ReportProgress(0, ProgressStatus);
            cancellationToken.ThrowIfCancellationRequested();
            DataSet data = db.GetTableData(detail, worker);
            totalTableCount = data.Tables.Count;
            totalJsonCount = data.Tables.Cast<DataTable>().Sum(x => x.Rows.Count);
            ProgressStatus.Status = "Converting Jsons...";
            worker.ReportProgress(0, ProgressStatus);
            cancellationToken.ThrowIfCancellationRequested();
            DataSet modifiedData = ConvertJson(data, detail.tableList, worker);
            ProgressStatus.Status = "Creating Json Tables in DB...";
            worker.ReportProgress(0, ProgressStatus);
            cancellationToken.ThrowIfCancellationRequested();
            db.CreateJsonTables(detail, modifiedData, worker);
            cancellationToken.ThrowIfCancellationRequested();
            db.DataSetBulkCopy(detail, modifiedData, worker);
        }
        private void DataSetCreation(DataSet set)
        {
            set.Tables.Add("ProceduresToMouth");
            set.Tables["ProceduresToMouth"].Columns.Add("Id", typeof(int));
            set.Tables["ProceduresToMouth"].Columns.Add("Arch", typeof(string));
            set.Tables["ProceduresToMouth"].Columns.Add("Note", typeof(string));
            set.Tables["ProceduresToMouth"].Columns.Add("Status", typeof(string));
            set.Tables["ProceduresToMouth"].Columns.Add("Deleted", typeof(bool));
            set.Tables["ProceduresToMouth"].Columns.Add("Quadrant", typeof(string));
            set.Tables["ProceduresToMouth"].Columns.Add("AddedLate", typeof(bool));
            set.Tables["ProceduresToMouth"].Columns.Add("Completed", typeof(int));
            set.Tables["ProceduresToMouth"].Columns.Add("Placement", typeof(string));
            set.Tables["ProceduresToMouth"].Columns.Add("Additional", typeof(bool));
            set.Tables["ProceduresToMouth"].Columns.Add("DeletedDate", typeof(DateTime));
            set.Tables["ProceduresToMouth"].Columns.Add("DiagnosisIds", typeof(string));
            set.Tables["ProceduresToMouth"].Columns.Add("ToothRegions", typeof(string));
            set.Tables["ProceduresToMouth"].Columns.Add("CompletedDate", typeof(DateTime));
            set.Tables["ProceduresToMouth"].Columns.Add("DeletedCompleted", typeof(bool));
            set.Tables["ProceduresToMouth"].Columns.Add("ToothChartId", typeof(int));

            set.Tables.Add("ProceduresToTeeth");
            set.Tables["ProceduresToTeeth"].Columns.Add("ToothChartId", typeof(int));
            set.Tables["ProceduresToTeeth"].Columns.Add("ToothId", typeof(int));
            set.Tables["ProceduresToTeeth"].Columns.Add("ProcedureTeethId", typeof(int));


            set.Tables.Add("Priority");
            set.Tables["Priority"].Columns.Add("Id", typeof(int));
            set.Tables["Priority"].Columns.Add("Name", typeof(string));
            set.Tables["Priority"].Columns.Add("ToothChartId", typeof(int));
            set.Tables["Priority"].Columns.Add("ProceduresToMouthId", typeof(int));
            set.Tables["Priority"].Columns.Add("ProcedureTeethId", typeof(int));

            set.Tables.Add("Procedure");
            set.Tables["Procedure"].Columns.Add("Id", typeof(int));
            set.Tables["Procedure"].Columns.Add("Code", typeof(string));
            set.Tables["Procedure"].Columns.Add("Value", typeof(string));
            set.Tables["Procedure"].Columns.Add("Description", typeof(string));
            set.Tables["Procedure"].Columns.Add("Downgradable", typeof(bool));
            set.Tables["Procedure"].Columns.Add("ProcedureGroup", typeof(string));
            set.Tables["Procedure"].Columns.Add("FriendlyDescription", typeof(string));
            set.Tables["Procedure"].Columns.Add("ToothChartId", typeof(int));
            set.Tables["Procedure"].Columns.Add("ProceduresToMouthId", typeof(int));
            set.Tables["Procedure"].Columns.Add("ProcedureTeethId", typeof(int));

            set.Tables.Add("InternalCode");
            set.Tables["InternalCode"].Columns.Add("Id", typeof(int));
            set.Tables["InternalCode"].Columns.Add("Code", typeof(string));
            set.Tables["InternalCode"].Columns.Add("Charge", typeof(float));
            set.Tables["InternalCode"].Columns.Add("Credit", typeof(float));
            set.Tables["InternalCode"].Columns.Add("System", typeof(bool));
            set.Tables["InternalCode"].Columns.Add("Deleted", typeof(bool));
            set.Tables["InternalCode"].Columns.Add("Required", typeof(bool));
            set.Tables["InternalCode"].Columns.Add("Description", typeof(string));
            set.Tables["InternalCode"].Columns.Add("InsurancePayment", typeof(bool));
            set.Tables["InternalCode"].Columns.Add("InternalCodeGroup", typeof(string));
            set.Tables["InternalCode"].Columns.Add("ToothChartId", typeof(int));
            set.Tables["InternalCode"].Columns.Add("ProceduresToMouthId", typeof(int));
            set.Tables["InternalCode"].Columns.Add("ProcedureTeethId", typeof(int));

            set.Tables.Add("Tooth");
            set.Tables["Tooth"].Columns.Add("Id", typeof(int));
            set.Tables["Tooth"].Columns.Add("Name", typeof(string));
            set.Tables["Tooth"].Columns.Add("Note", typeof(string));
            set.Tables["Tooth"].Columns.Add("IsDeleted", typeof(bool));
            set.Tables["Tooth"].Columns.Add("ToothChartId", typeof(int));
            set.Tables["Tooth"].Columns.Add("ProcedureToMouthId", typeof(int));
            set.Tables["Tooth"].Columns.Add("ProcedureTeethId", typeof(int));

            set.Tables.Add("Position");
            set.Tables["Position"].Columns.Add("Number", typeof(int));
            set.Tables["Position"].Columns.Add("Quadrant", typeof(string));
            set.Tables["Position"].Columns.Add("Deciduous", typeof(bool));
            set.Tables["Position"].Columns.Add("TeethType", typeof(string));
            set.Tables["Position"].Columns.Add("ToothChartId", typeof(int));
            set.Tables["Position"].Columns.Add("toothId", typeof(int));
            set.Tables["Position"].Columns.Add("procedureTeethToothId", typeof(int));

            set.Tables.Add("ToothStatus");
            set.Tables["ToothStatus"].Columns.Add("Id", typeof(int));
            set.Tables["ToothStatus"].Columns.Add("Implant", typeof(bool));
            set.Tables["ToothStatus"].Columns.Add("Impacted", typeof(bool));
            set.Tables["ToothStatus"].Columns.Add("Mobility", typeof(bool));
            set.Tables["ToothStatus"].Columns.Add("Furcation", typeof(bool));
            set.Tables["ToothStatus"].Columns.Add("GumRegions", typeof(string));
            set.Tables["ToothStatus"].Columns.Add("MobilityLevel", typeof(string));
            set.Tables["ToothStatus"].Columns.Add("FurcationLevel", typeof(string));
            set.Tables["ToothStatus"].Columns.Add("SpaceMaintainer", typeof(bool));
            set.Tables["ToothStatus"].Columns.Add("TissueRecession", typeof(bool));
            set.Tables["ToothStatus"].Columns.Add("MarkBleedingGums", typeof(bool));
            set.Tables["ToothStatus"].Columns.Add("PeriodontalDisease", typeof(bool));
            set.Tables["ToothStatus"].Columns.Add("TissueRecessionLevel", typeof(string));
            set.Tables["ToothStatus"].Columns.Add("PeriodontalDiseaseLevel", typeof(string));
            set.Tables["ToothStatus"].Columns.Add("ToothChartId", typeof(int));
            set.Tables["ToothStatus"].Columns.Add("ToothId", typeof(int));
            set.Tables["ToothStatus"].Columns.Add("ProcedureTeethId", typeof(int));

            set.Tables.Add("ProcedureTeeth");
            set.Tables["ProcedureTeeth"].Columns.Add("Id", typeof(int));
            set.Tables["ProcedureTeeth"].Columns.Add("Arch", typeof(string));
            set.Tables["ProcedureTeeth"].Columns.Add("Note", typeof(string));
            set.Tables["ProcedureTeeth"].Columns.Add("Status", typeof(string));
            set.Tables["ProcedureTeeth"].Columns.Add("Deleted", typeof(bool));
            set.Tables["ProcedureTeeth"].Columns.Add("Quadrant", typeof(string));
            set.Tables["ProcedureTeeth"].Columns.Add("AddedLate", typeof(bool));
            set.Tables["ProcedureTeeth"].Columns.Add("Completed", typeof(int));
            set.Tables["ProcedureTeeth"].Columns.Add("Placement", typeof(string));
            set.Tables["ProcedureTeeth"].Columns.Add("Additional", typeof(bool));
            set.Tables["ProcedureTeeth"].Columns.Add("DeletedDate", typeof(DateTime));
            set.Tables["ProcedureTeeth"].Columns.Add("DiagnosisIds", typeof(string));
            set.Tables["ProcedureTeeth"].Columns.Add("ToothRegions", typeof(string));
            set.Tables["ProcedureTeeth"].Columns.Add("CompletedDate", typeof(DateTime));
            set.Tables["ProcedureTeeth"].Columns.Add("DeletedCompleted", typeof(bool));
            set.Tables["ProcedureTeeth"].Columns.Add("ToothChartId", typeof(int));

            set.Tables.Add("DeletedDoctor");
            set.Tables["DeletedDoctor"].Columns.Add("Id", typeof(int));
            set.Tables["DeletedDoctor"].Columns.Add("FullName", typeof(string));
            set.Tables["DeletedDoctor"].Columns.Add("LastName", typeof(string));
            set.Tables["DeletedDoctor"].Columns.Add("FirstName", typeof(string));
            set.Tables["DeletedDoctor"].Columns.Add("MiddleName", typeof(string));
            set.Tables["DeletedDoctor"].Columns.Add("ProcedureTeethId", typeof(int));

            set.Tables.Add("GumMeasurementDtos");
            set.Tables["GumMeasurementDtos"].Columns.Add("Id", typeof(int));
            set.Tables["GumMeasurementDtos"].Columns.Add("ToothId", typeof(int));
            set.Tables["GumMeasurementDtos"].Columns.Add("LeftGumMeasurement", typeof(int));
            set.Tables["GumMeasurementDtos"].Columns.Add("RightGumMeasurement", typeof(int));
            set.Tables["GumMeasurementDtos"].Columns.Add("MiddleGumMeasurement", typeof(int));
            set.Tables["GumMeasurementDtos"].Columns.Add("InnerLeftGumMeasurement", typeof(int));
            set.Tables["GumMeasurementDtos"].Columns.Add("InnerRightGumMeasurement", typeof(int));
            set.Tables["GumMeasurementDtos"].Columns.Add("InnerMiddleGumMeasurement", typeof(int));
            set.Tables["GumMeasurementDtos"].Columns.Add("PerioHistoryId", typeof(int));


        }
        public DataSet ConvertJson(DataSet data, string[] tableList, BackgroundWorker worker)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DataSet set = new DataSet();
            DataSetCreation(set);
            long id = 0;
            try
            {
                foreach (string tableName in tableList)
                {
                    object lockObject = new object();
                    tableCount++;
                    if (tableName == "tooth_chart_history")
                    {

                        Parallel.ForEach(data.Tables["tooth_chart_history"].AsEnumerable(), row =>
                        {
                            new ParallelOptions
                            {
                                MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 2.0))
                            };
                            cancellationToken.ThrowIfCancellationRequested();
                            rowindex++;
                            string json = row["tooth_chart"].ToString();
                            id = (long)row["id"];
                            var jObject = JObject.Parse(json);
                            foreach (var jO in jObject.Children())
                            {
                                if (jO.Path == "proceduresToMouth")
                                {
                                    JToken pTM = jO;
                                    if (pTM.First.HasValues)
                                    {
                                        foreach (var x in pTM.Children())
                                        {
                                            ProceduresToMouth[] j = JsonConvert.DeserializeObject<ProceduresToMouth[]>(x.ToString());
                                            foreach (var procToMouth in j)
                                            {
                                                procToMouth.toothChartId = (int)id;
                                                lock (lockObject)
                                                {
                                                    set.Tables["ProceduresToMouth"].Rows.Add(procToMouth.id, procToMouth.arch, procToMouth.note, procToMouth.status, procToMouth.deleted, procToMouth.quadrant, procToMouth.addedLate, procToMouth.completed, procToMouth.placement, procToMouth.additional, procToMouth.deletedDate.HasValue ? EpochToDateTimeConverter(procToMouth.deletedDate) : DBNull.Value, procToMouth.toothRegions.IsNullOrEmpty() ? null : string.Join(string.Empty, procToMouth.toothRegions).ToString(), procToMouth.diagnosisIds.IsNullOrEmpty() ? null : string.Join(string.Empty, procToMouth.diagnosisIds).ToString(), procToMouth.completedDate.HasValue ? EpochToDateTimeConverter(procToMouth.completedDate) : DBNull.Value, procToMouth.deletedCompleted, procToMouth.toothChartId);
                                                }
                                                if (procToMouth.priority != null)
                                                {
                                                    Priority priority = procToMouth.priority;
                                                    priority.toothChartId = (int)id;
                                                    priority.proceduresToMouthId = procToMouth.id;
                                                    lock (lockObject)
                                                    {
                                                        set.Tables["Priority"].Rows.Add(priority.id, priority.name, priority.toothChartId, priority.proceduresToMouthId, null);
                                                    }
                                                }
                                                if (procToMouth.procedure != null)
                                                {
                                                    Procedure procedure = procToMouth.procedure;
                                                    procedure.toothChartId = (int)id;
                                                    procedure.proceduresToMouthId = procToMouth.id;
                                                    lock (lockObject)
                                                    {
                                                        set.Tables["Procedure"].Rows.Add(procedure.id, procedure.code, procedure.value, procedure.description, procedure.downgradable, procedure.procedureGroup, procedure.friendlyDescription, procedure.toothChartId, procedure.proceduresToMouthId, null);
                                                    }
                                                }
                                                if (procToMouth.deletedByDoctor != null)
                                                {
                                                    DeletedDoctor deletedDoctor = procToMouth.deletedByDoctor;
                                                    deletedDoctor.procedureTeethId = procToMouth.id;
                                                    lock (lockObject)
                                                    {
                                                        set.Tables["DeletedDoctor"].Rows.Add(deletedDoctor.id, deletedDoctor.fullName, deletedDoctor.lastName, deletedDoctor.firstName, deletedDoctor.middleName, deletedDoctor.procedureTeethId);
                                                    }
                                                }
                                                if (procToMouth.internalCode != null)
                                                {
                                                    Internalcode internalCode = procToMouth.internalCode;
                                                    internalCode.toothChartId = (int)id;
                                                    internalCode.proceduresToMouthId = procToMouth.id;
                                                    lock (lockObject)
                                                    {
                                                        set.Tables["InternalCode"].Rows.Add(internalCode.id, internalCode.code, internalCode.charge, internalCode.credit, internalCode.system, internalCode.deleted, internalCode.required, internalCode.description, internalCode.insurancePayment, internalCode.internalCodeGroup, internalCode.toothChartId, internalCode.proceduresToMouthId, null);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }
                                else if (jO.Path == "proceduresToTeeth")
                                {
                                    JToken pTT = jO;
                                    if (pTT.First.HasValues)
                                    {
                                        foreach (var x in pTT.Children())
                                        {
                                            ProceduresToTeeth[] k = JsonConvert.DeserializeObject<ProceduresToTeeth[]>(x.ToString());
                                            foreach (var procToTeeth in k)
                                            {
                                                procToTeeth.toothChartId = (int)id;
                                                procToTeeth.toothId = procToTeeth.tooth.id;
                                                if (procToTeeth.procedureTeeth.IsNullOrEmpty())
                                                {
                                                    lock (lockObject)
                                                    {
                                                        set.Tables["ProceduresToTeeth"].Rows.Add(procToTeeth.toothChartId, procToTeeth.toothId, null);
                                                    }
                                                }
                                                else
                                                {
                                                    foreach (var y in procToTeeth.procedureTeeth)
                                                    {
                                                        lock (lockObject)
                                                        {
                                                            set.Tables["ProceduresToTeeth"].Rows.Add(procToTeeth.toothChartId, procToTeeth.toothId, y.id);
                                                        }
                                                    }
                                                }
                                                if (procToTeeth.tooth != null)
                                                {
                                                    Tooth tooth = procToTeeth.tooth;
                                                    tooth.toothChartId = (int)id;
                                                    lock (lockObject)
                                                    {
                                                        set.Tables["Tooth"].Rows.Add(tooth.id, tooth.name, tooth.note, tooth.isDeleted, tooth.toothChartId, null, null);
                                                    }
                                                    if (tooth.position != null)
                                                    {
                                                        Position position = tooth.position;
                                                        position.toothChartId = (int)id;
                                                        position.toothId = tooth.id;
                                                        position.procedureTeethToothId = tooth.id;
                                                        lock (lockObject)
                                                        {
                                                            set.Tables["Position"].Rows.Add(position.number, position.quadrant, position.deciduous, position.teethType, position.toothChartId, position.toothId, null);
                                                        }
                                                    }
                                                    if (tooth.toothStatus != null)
                                                    {
                                                        Toothstatus toothstatus = tooth.toothStatus;
                                                        toothstatus.toothChartId = (int)id;
                                                        toothstatus.toothId = tooth.id;
                                                        lock (lockObject)
                                                        {
                                                            set.Tables["ToothStatus"].Rows.Add(toothstatus.id, toothstatus.implant, toothstatus.impacted, toothstatus.mobility, toothstatus.furcation, toothstatus.gumRegions, toothstatus.mobilityLevel, toothstatus.furcationLevel, toothstatus.spaceMaintainer, toothstatus.tissueRecession, toothstatus.markBleedingGums, toothstatus.periodontalDisease, toothstatus.tissueRecessionLevel, toothstatus.periodontalDiseaseLevel, toothstatus.toothChartId, toothstatus.toothId, null);
                                                        }
                                                    }
                                                }
                                                if (procToTeeth.procedureTeeth != null)
                                                {
                                                    ProcedureTeeth[] procedureTeeth = procToTeeth.procedureTeeth;
                                                    foreach (var item in procedureTeeth)
                                                    {
                                                        lock (lockObject)
                                                        {
                                                            set.Tables["ProcedureTeeth"].Rows.Add(item.id, item.arch, item.note, item.status, item.deleted, item.quadrant, item.addedLate, item.completed, item.placement, item.additional, item.deletedDate.HasValue == true ? EpochToDateTimeConverter((long)item.deletedDate) : null, item.toothRegions.IsNullOrEmpty() ? null : string.Join(string.Empty, item.toothRegions).ToString(), item.diagnosisIds.IsNullOrEmpty() ? null : string.Join(string.Empty, item.diagnosisIds).ToString(), item.completedDate.HasValue == true ? EpochToDateTimeConverter((long)item.completedDate) : null, item.deletedCompleted, id);
                                                        }
                                                        if (item.tooth != null)
                                                        {
                                                            Tooth tooth = item.tooth;
                                                            tooth.toothChartId = (int)id;
                                                            tooth.procedureTeethId = item.id;
                                                            lock (lockObject)
                                                            {
                                                                set.Tables["Tooth"].Rows.Add(tooth.id, tooth.name, tooth.note, tooth.isDeleted, tooth.toothChartId, null, item.id);
                                                            }
                                                            if (tooth.position != null)
                                                            {
                                                                Position position = tooth.position;
                                                                position.toothChartId = (int)id;
                                                                position.procedureTeethToothId = tooth.id;
                                                                lock (lockObject)
                                                                {
                                                                    set.Tables["Position"].Rows.Add(position.number, position.quadrant, position.deciduous, position.teethType, position.toothChartId, null, position.procedureTeethToothId);
                                                                }

                                                            }
                                                            if (tooth.toothStatus != null)
                                                            {
                                                                Toothstatus toothstatus = tooth.toothStatus;
                                                                toothstatus.toothChartId = (int)id;
                                                                toothstatus.toothId = tooth.id;
                                                                lock (lockObject)
                                                                {
                                                                    set.Tables["ToothStatus"].Rows.Add(toothstatus.id, toothstatus.implant, toothstatus.impacted, toothstatus.mobility, toothstatus.furcation, toothstatus.gumRegions, toothstatus.mobilityLevel, toothstatus.furcationLevel, toothstatus.spaceMaintainer, toothstatus.tissueRecession, toothstatus.markBleedingGums, toothstatus.periodontalDisease, toothstatus.tissueRecessionLevel, toothstatus.periodontalDiseaseLevel, toothstatus.toothChartId, null, toothstatus.toothId);
                                                                }
                                                            }
                                                        }
                                                        if (item.priority != null /*&& item.priority.id != null*/)
                                                        {
                                                            if (item.priority.GetType() == typeof(JObject))
                                                            {
                                                                Priority priority = JsonConvert.DeserializeObject<Priority>(item.priority.ToString());
                                                                priority.toothChartId = (int)id;
                                                                priority.procedureTeethId = item.id;
                                                                lock (lockObject)
                                                                {
                                                                    set.Tables["Priority"].Rows.Add(priority.id, priority.name, priority.toothChartId, null, priority.procedureTeethId);
                                                                }
                                                            }
                                                        }
                                                        if (item.procedure != null)
                                                        {
                                                            Procedure procedure = item.procedure;
                                                            procedure.toothChartId = (int)id;
                                                            procedure.procedureTeethId = item.id;
                                                            lock (lockObject)
                                                            {
                                                                set.Tables["Procedure"].Rows.Add(procedure.id, procedure.code, procedure.value, procedure.description, procedure.downgradable, procedure.procedureGroup, procedure.friendlyDescription, procedure.toothChartId, null, procedure.procedureTeethId);
                                                            }
                                                        }
                                                        if (item.deletedByDoctor != null)
                                                        {
                                                            DeletedDoctor deletedDoctor = item.deletedByDoctor;
                                                            deletedDoctor.procedureTeethId = item.id;
                                                            lock (lockObject)
                                                            {
                                                                set.Tables["DeletedDoctor"].Rows.Add(deletedDoctor.id, deletedDoctor.fullName, deletedDoctor.lastName, deletedDoctor.firstName, deletedDoctor.middleName, deletedDoctor.procedureTeethId);
                                                            }
                                                        }
                                                        if (item.internalCode != null)
                                                        {
                                                            Internalcode internalCode = item.internalCode;
                                                            internalCode.toothChartId = (int)id;
                                                            internalCode.procedureteethId = item.id;
                                                            lock (lockObject)
                                                            {
                                                                set.Tables["InternalCode"].Rows.Add(internalCode.id, internalCode.code, internalCode.charge, internalCode.credit, internalCode.system, internalCode.deleted, internalCode.required, internalCode.description, internalCode.insurancePayment, internalCode.internalCodeGroup, internalCode.toothChartId, null, internalCode.procedureteethId);
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            worker.ReportProgress(GetProgressPercentage(tableCount, totalTableCount, rowindex, totalJsonCount));
                        });
                    }
                    else if (tableName == "perio_history")
                    {
                        Parallel.ForEach(data.Tables["perio_history"].AsEnumerable(), row =>
                        {
                            new ParallelOptions
                            {
                                MaxDegreeOfParallelism = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 2.0))
                            };
                            cancellationToken.ThrowIfCancellationRequested();
                            rowindex++;
                            string json = row["gum_measurement"].ToString();
                            id = (long)row["id"];
                            var jObject = JObject.Parse(json);
                            foreach (var jO in jObject.Children())
                            {
                                JToken gMD = jO;
                                foreach (var c in gMD.Children())
                                {
                                    GumMeasurementDtos[] j = JsonConvert.DeserializeObject<GumMeasurementDtos[]>(c.ToString());
                                    foreach (var gumMeasurementDtos in j)
                                    {
                                        gumMeasurementDtos.perioHistoryId = (int)id;
                                        lock (lockObject)
                                        {
                                            set.Tables["GumMeasurementDtos"].Rows.Add(gumMeasurementDtos.id, gumMeasurementDtos.toothid, gumMeasurementDtos.leftGumMeasurement, gumMeasurementDtos.rightGumMeasurement, gumMeasurementDtos.middleGumMeasurement, gumMeasurementDtos.innerLeftGumMeasurement, gumMeasurementDtos.innerRightGumMeasurement, gumMeasurementDtos.innerMiddleGumMeasurement, gumMeasurementDtos.perioHistoryId);
                                        }
                                    }
                                }
                            }
                            worker.ReportProgress(GetProgressPercentage(tableCount, totalTableCount, rowindex, totalJsonCount));
                        });
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(id);
            }

            return set;
        }

        private DateTime EpochToDateTimeConverter(long? date)
        {
            DateTime convertedDate;
            DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds((long)date);
            convertedDate = dateTimeOffset.DateTime;
            return convertedDate;
        }

        private int GetProgressPercentage(int tableCount, int totalTableCount, int rowindex, int totalJsonCount)
        {
            return rowindex * 100 / totalJsonCount;
        }
        public void CancelTask()
        {
            CancellationTokenSource.Cancel();
        }

    }
}