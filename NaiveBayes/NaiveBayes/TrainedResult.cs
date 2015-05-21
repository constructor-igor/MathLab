using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NaiveBayes
{
    public class TrainedResult
    {
        public ConcurrentDictionary<string, Dictionary<string, MeanVariance>> Features = new ConcurrentDictionary<string, Dictionary<string, MeanVariance>>();

        public void AddStatistics(string classificator, string feature, MeanVariance meanVariance)
        {
            if (!Features.ContainsKey(classificator))
                Features.TryAdd(classificator, new Dictionary<string, MeanVariance>());
            Features[classificator].Add(feature, meanVariance);
        }
    }
}