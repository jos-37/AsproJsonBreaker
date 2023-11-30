using AsproRDTool.ServiceContracts.Models;
using System.ComponentModel;

namespace AsproRDTool.ServiceContracts
{
    public interface IJsonBreaker
    {
        JsonBreakerStatus ProgressStatus { get; set; }
        void InitiateJsonModification(JsonBreakerDetail detail, BackgroundWorker worker, DoWorkEventArgs e);
        CancellationTokenSource CancellationTokenSource { get; set; }
        void CancelTask();
    }
}
