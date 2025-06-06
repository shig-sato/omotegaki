using OmoSeitoku;
using OmoSeitokuEreceipt.SER;

namespace OmoOmotegaki.Models
{
    public interface IShinryouDataCollectionSource
    {
        ShinryouDataCollection ShinryouDataCollection { get; }

        DateRange? CurrentDateRange { get; }
    }
}
