using Serilog;

namespace RelatedRows.Domain
{
    public interface IDatasetConsumer: IActionable<CDataset>
    {
        void OnDatasetUpdated(CDataset dataSet);
        void OnConnected(CConfiguration config, ILogger logger);
    }
}
