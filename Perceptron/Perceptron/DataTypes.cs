using System.Collections.Generic;

namespace Perceptron
{
    public class TrainingData
    {
        public readonly TestData TestData = new TestData();
        public readonly ReferenceData Reference = new ReferenceData();
    }
    public class TestData
    {
        public List<DataItem> Items = new List<DataItem>();
    }
    public class DataItem
    {
        public readonly List<double> Values = new List<double>();
        public DataItem(double[] values)
        {
            Values.AddRange(values);
        }
    }
    public class ReferenceData
    {
        public readonly List<double> Values = new List<double>();
        public void Add(double[] values)
        {
            Values.AddRange(values);
        }
    }
}
