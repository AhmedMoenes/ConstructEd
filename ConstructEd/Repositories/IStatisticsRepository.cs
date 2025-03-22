using ConstructEd.ViewModels;

namespace ConstructEd.Repositories
{
    public interface IStatisticsRepository
    {
        Task<StatisticsViewModel> GetStatisticsAsync();
    }
}
