using Serilog;

namespace RelatedRows.Domain
{
    public interface ITableConsumer: IActionable<CTable>
    {
        void OnTableUpdated(CTable table);
        void OnConnected(CConfiguration config, ILogger logger);
    }
}
