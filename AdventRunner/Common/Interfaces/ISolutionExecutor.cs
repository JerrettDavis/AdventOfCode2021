using System.Threading.Tasks;

namespace AdventRunner.Common.Interfaces
{
    public interface ISolutionExecutor
    {
        string Key { get; }
        Task<ISolution> GetSolutionAsync();
    }
}