using System.Collections.Generic;

namespace NaiveBayes
{
    public class DataSet
    {
        public DataSetDescriptor Descriptor { get; private set; }
        public readonly List<DataSetItem> Items = new List<DataSetItem>();

        public DataSet(DataSetDescriptor descriptor)
        {
            Descriptor = descriptor;
        }

        public void Add(DataSetItem dataItem)
        {
            Items.Add(dataItem);
            Descriptor.AddClassificator(dataItem.Classificator);
        }
    }

    public class DataSetDescriptor
    {
        public string Classificator { get; set; }
        public readonly List<string> ClassificatorValues =new List<string>();
        public readonly List<string> Features = new List<string>();

        public void AddClassificator(string classificator)
        {
            if (string.IsNullOrWhiteSpace(classificator))
                return;
            if (ClassificatorValues.Contains(classificator))
                return;
            ClassificatorValues.Add(classificator);
        }
    }

    public class DataSetItem
    {
        public readonly List<double> Features = new List<double>();
        public string Classificator { get; set; }
    }
}