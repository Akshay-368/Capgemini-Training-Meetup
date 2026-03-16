using QuantityMeasurement.Application.Models;

namespace QuantityMeasurement.Application.Interfaces;

public interface IHistoryRepository
{
    void Save(HistoryRecord history);

    List<HistoryRecord> GetHistory();

    void ClearHistory();
}