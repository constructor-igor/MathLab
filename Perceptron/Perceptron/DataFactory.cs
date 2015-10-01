namespace Perceptron
{
    public class DataFactory
    {
        public static TrainingData CreateTrainingData()
        {
            TrainingData trainingData = new TrainingData();
            trainingData.TestData.Items.Add(new DataItem(new double[] { 1, 0, 0 }));
            trainingData.TestData.Items.Add(new DataItem(new double[] { 1, 0, 1 }));
            trainingData.TestData.Items.Add(new DataItem(new double[] { 1, 1, 0 }));
            trainingData.TestData.Items.Add(new DataItem(new double[] { 1, 1, 1 }));
            trainingData.Reference.Add(new double[] { 1, 1, 1, 0 });
            return trainingData;
        }
    }
}