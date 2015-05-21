using System;
using System.Threading.Tasks;

namespace NaiveBayes
{
    class Program
    {
        static void Main(string[] args)
        {
            DataSet trainingSet = DataSetActions.LoadFromFile("trainingSet.csv");
            DataSetActions.WriteToConsole(trainingSet);
            TrainedResult trainedResult = DataSetActions.Training(trainingSet);
            DataSetActions.WriteToConsole(trainedResult);

            DataSetItem examDataSetItem = new DataSetItem();
            examDataSetItem.Features.Add(6);    // height
            examDataSetItem.Features.Add(130);  // weight
            examDataSetItem.Features.Add(8);    // foot size

            double maleResult = 0;
            double femaleResult = 0;

            ParallelOptions parallelOptions = new ParallelOptions();
            Parallel.Invoke(parallelOptions,
                () => { maleResult = DataSetActions.CalculateForClassificator(trainedResult, trainingSet.Descriptor, examDataSetItem, "Male");},
                () => { femaleResult = DataSetActions.CalculateForClassificator(trainedResult, trainingSet.Descriptor, examDataSetItem, "Female"); });

            Console.WriteLine("male: {0}", maleResult);
            Console.WriteLine("female: {0}", femaleResult);
        }
    }
}
