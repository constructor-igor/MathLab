using System.Threading.Tasks;
using NUnit.Framework;

namespace NaiveBayes
{
    [TestFixture]
    public class AcceptanceTests
    {
        [Test]
        [Repeat(10000)]
        public void SanityTest()
        {
            DataSet trainingSet = DataSetActions.LoadFromFile("trainingSet.csv");
            TrainedResult trainedResult = DataSetActions.Training(trainingSet);

            DataSetItem examDataSetItem = new DataSetItem();
            examDataSetItem.Features.Add(6);    // height
            examDataSetItem.Features.Add(130);  // weight
            examDataSetItem.Features.Add(8);    // foot size

            double maleResult = 0;
            double femaleResult = 0;

            ParallelOptions parallelOptions = new ParallelOptions();
            Parallel.Invoke(parallelOptions,
                () => { maleResult = DataSetActions.CalculateForClassificator(trainedResult, trainingSet.Descriptor, examDataSetItem, "Male"); },
                () => { femaleResult = DataSetActions.CalculateForClassificator(trainedResult, trainingSet.Descriptor, examDataSetItem, "Female"); });

            Assert.That(maleResult, Is.EqualTo(0.997981505982277).Within(0.00001));
            Assert.That(femaleResult, Is.EqualTo(0.0020184940177232).Within(0.00001));
        }
    }
}