using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace NaiveBayes
{
    public class DataSetActions
    {
        public static DataSet LoadFromFile(string pathToFile)
        {
            DataSetDescriptor descriptor = new DataSetDescriptor();

            string[] lines = File.ReadAllLines(pathToFile);
            string header = lines[0];
            string[] headerItems = header.Split(',');
            descriptor.Classificator = headerItems[0];
            for (int i = 1; i < headerItems.Length; i++)
            {
                descriptor.Features.Add(headerItems[i]);
            }

            DataSet dataSet = new DataSet(descriptor);
            DataSetItem[] items = new DataSetItem[lines.Length];
            Parallel.For(1, lines.Length, i =>
            {
                string[] dataItems = lines[i].Split(',');

                DataSetItem dataItem = new DataSetItem {Classificator = dataItems[0]};
                for (int j = 1; j < dataItems.Length; j++)
                {
                    dataItem.Features.Add(Double.Parse(dataItems[j]));
                }
                //lock(Lock)
                //    dataSet.Add(dataItem);
                items[i] = dataItem;
            });
            dataSet.Add(items);

            return dataSet;
        }

        public static void WriteToConsole(DataSet dataSet)
        {            
            const string FORMAT = "{0}: {1}";

            Console.WriteLine();
            Console.WriteLine(FORMAT, dataSet.Descriptor.Classificator, String.Join(", ", dataSet.Descriptor.Features));
            foreach (DataSetItem dataItem in dataSet.Items)
            {
                Console.WriteLine(FORMAT, dataItem.Classificator, String.Join(", ", dataItem.Features));
            }
        }

        public static TrainedResult Training(DataSet dataSet)
        {
            TrainedResult trainedResult = new TrainedResult();
            List<string> allClassificators = dataSet.Descriptor.ClassificatorValues;
            Parallel.ForEach(allClassificators, classificator =>
            {
                List<DataSetItem> subSet = dataSet.Items.Where(i => i.Classificator == classificator).ToList();

                for (int index = 0; index < dataSet.Descriptor.Features.Count; index++)
                {
                    List<double> featureValues = subSet.Select(dataSetItem => dataSetItem.Features[index]).ToList();
                    MeanVariance meanVariance = featureValues.CalculateMeanVariance();
                    trainedResult.AddStatistics(classificator, dataSet.Descriptor.Features[index], meanVariance);
                }
            });
            return trainedResult;
        }

        public static void WriteToConsole(TrainedResult trainedResult)
        {
            Console.WriteLine();
            foreach (KeyValuePair<string, Dictionary<string, MeanVariance>> keyValuePair in trainedResult.Features)
            {
                Console.WriteLine("classificator: {0}", keyValuePair.Key);
                foreach (KeyValuePair<string, MeanVariance> meanVariance in keyValuePair.Value)
                {
                    Console.WriteLine("feature {0}: mean = {1}, variance = {2}", meanVariance.Key, meanVariance.Value.Mean, meanVariance.Value.Variance);
                }
            }
        }

        public static double CalculateForClassificator(TrainedResult trainingResult, DataSetDescriptor descriptor, DataSetItem examDataSetItem, string classificator)
        {
            ConcurrentDictionary<string, double> evidencePerClassificator = new ConcurrentDictionary<string, double>();

            double evidence = 0;
            Parallel.ForEach(descriptor.ClassificatorValues, classificatorValue =>
            {
                double evidenceClassificator = 0.5;
                Dictionary<string, MeanVariance> classificatorTrainedResult = trainingResult.Features[classificatorValue];

                for (int index = 0; index < classificatorTrainedResult.Keys.Count; index++)
                {
                    string key = classificatorTrainedResult.Keys.ToList()[index];
                    MeanVariance featureMeanVariance = classificatorTrainedResult[key];
                    evidenceClassificator *= MathStatistics.CalcP(featureMeanVariance, examDataSetItem.Features[index]);
                }
                evidencePerClassificator[classificatorValue] = evidenceClassificator;
                InterlockedEx.Add(ref evidence, evidenceClassificator);
            });

            double result = evidencePerClassificator[classificator]/evidence;
            return result;
        }
    }
}