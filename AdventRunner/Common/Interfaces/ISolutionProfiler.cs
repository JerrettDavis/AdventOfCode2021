using System.Collections.Generic;

namespace AdventRunner.Common.Interfaces
{
    public interface ISolutionProfiler
    {
        void Start();
        void MarkFileReadComplete();
        void MarkFileParseComplete();
        void MarkPartAComplete();
        void MarkParkBComplete();
        void PrintStats();
        Dictionary<string, double> GetStats();
    }
}