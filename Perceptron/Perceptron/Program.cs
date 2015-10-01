using System;

namespace Perceptron
{
    class Program
    {
        static void Main(string[] args)
        {
            TrainingData trainingData = DataFactory.CreateTrainingData();
            PerceptronAlgorithm perceptronAlgorithm = new PerceptronAlgorithm();
            perceptronAlgorithm.Training(trainingData);

            Console.WriteLine(trainingData.TestData.Items.Count);
        }
    }

    public class PerceptronAlgorithm
    {
        private int m_sensorsCount;
        private double[] m_InitialWeights;
        private double[] m_FinalWeights;
        private PerceptionOutput m_perceptionOutput;
        private double m_error;
        private double m_correction;
        private TrainingData m_trainingData;

        public void Training(TrainingData trainingData)
        {
            m_trainingData = trainingData;
            TrainingInitialization(trainingData);
        }

        private void TrainingInitialization(TrainingData trainingData)
        {
            m_sensorsCount = trainingData.TestData.Items[0].Values.Count;
            m_InitialWeights = new double[m_sensorsCount];
            m_FinalWeights = new double[m_sensorsCount];
            m_perceptionOutput = new PerceptionOutput(m_sensorsCount);
            m_error = 0;
            m_correction = 0;
        }
    }
    public class PerceptionOutput
    {
        private double[] m_coefficientsC;
        private double m_summa;
        private bool m_network;

        public PerceptionOutput(int sensorsCount)
        {
            m_coefficientsC = new double[sensorsCount];
        }
    }
}
