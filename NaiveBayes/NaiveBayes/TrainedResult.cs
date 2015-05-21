using System.Collections.Generic;

namespace NaiveBayes
{
    public class TrainedResult
    {
        public Dictionary<string, Dictionary<string, MeanVariance>> Features = new Dictionary<string, Dictionary<string, MeanVariance>>();

        public void AddStatistics(string classificator, string feature, MeanVariance meanVariance)
        {
            if (!Features.ContainsKey(classificator))
                Features.Add(classificator, new Dictionary<string, MeanVariance>());
            Features[classificator].Add(feature, meanVariance);
        }
    }
}