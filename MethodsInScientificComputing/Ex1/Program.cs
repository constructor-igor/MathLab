using System;
using System.Collections.Generic;
using System.Linq;

namespace Ex1
{
    class Program
    {
        static void Main()
        {
            SinglePopulationContinuousExperiment(new SinglePopulationDefinition { X0 = 1000, K = 0.1}, new ExperimentDefinition{M=1000000, DeltaM = 1});
            SinglePopulationContinuousExperiment(new SinglePopulationDefinition { X0 = 1, K = 1}, new ExperimentDefinition { M = 10, DeltaM = 1 });

            var populations = SinglePopulationDefinition.New(3, X0: 1000, k: 0.1);
            MultiPopulationContinuousExperiment(populations, new ExperimentDefinition {M = 1000000, DeltaM = 1});
        }

        private static void SinglePopulationContinuousExperiment(SinglePopulationDefinition singlePopulationDefinition, ExperimentDefinition experimentDefinition)
        {            
            double X0 = singlePopulationDefinition.X0;
            double growthRateK = singlePopulationDefinition.K;

            double M = experimentDefinition.M;
            double deltaM = experimentDefinition.DeltaM;

            Console.WriteLine();
            Console.WriteLine("M={0}, X0={1}, k={2}, deltaM={3}", M, X0, growthRateK, deltaM);

            var singleContinuousPopulation = new SingleContinuousPopulation(X0, growthRateK);
            double analyticalSolvedTime = singleContinuousPopulation.CalculateT(M);
            Console.WriteLine("analytical calculated t = {0}, population = {1}", analyticalSolvedTime, singleContinuousPopulation.X(analyticalSolvedTime));

            double X = X0;
            double t = 0;
            int iterations = 0;
            while (Math.Abs(X - M) > deltaM)
            {
                t = t + singleContinuousPopulation.CalculateDeltaT(deltaM, t);
                X = singleContinuousPopulation.X(t);
                iterations++;
            }

            Console.WriteLine("simulation calculated t = {0}, population = {1}, iterations = {2}", 
                t, singleContinuousPopulation.X(t), iterations);
        }

        private static void MultiPopulationContinuousExperiment(List<SinglePopulationDefinition> definitions, ExperimentDefinition experimentDefinition)
        {
            double M = experimentDefinition.M;
            double deltaM = experimentDefinition.DeltaM;

            Console.WriteLine();
            Console.WriteLine("multi population experiment");
            Console.WriteLine("M={0}, deltaM={1}", M, deltaM);

            double singleDeltaM = deltaM/definitions.Count;
            var populations = definitions.Select(definition => new SingleContinuousPopulation(definition.X0, definition.K)).ToList();

            double X = definitions.Sum(d => d.X0);
            double t = 0;
            int iterations = 0;
            while (Math.Abs(X - M) > deltaM)
            {
                t += populations.Min(d => d.CalculateDeltaT(singleDeltaM, t));
                X = populations.Sum(d => d.X(t));
                iterations++;
            }

            Console.WriteLine("simulation calculated t = {0}, populations = {1}, iterations = {2}",
                t, populations.Sum(d => d.X(t)), iterations);
        }
    }

    public class SinglePopulationDefinition
    {        
        public double X0 { get; set; }
        public double K { get; set; }

        public static List<SinglePopulationDefinition> New(int numberOfPopulations, double X0, double k)
        {
            var list = new List<SinglePopulationDefinition>();
            for (int i = 0; i < numberOfPopulations; i++)
            {
                list.Add(new SinglePopulationDefinition{X0=X0, K=k});
            }
            return list;
        }
    }

    public class ExperimentDefinition
    {
        public double M { get; set; }
        public double DeltaM { get; set; }
    }
    public class SingleContinuousPopulation
    {
        private readonly double growthRateK;
        private readonly double x0;
        public SingleContinuousPopulation(double x0, double growthRateK)
        {
            this.growthRateK = growthRateK;
            this.x0 = x0;
        }

        // X(t) = Xo * e^kt
        public double X(double t)
        {
            return x0*Math.Pow(Math.E, growthRateK*t);
        }

        public double CalculateT(double M)
        {
            return (1/growthRateK)*Math.Log(M/x0);
        }

        public double CalculateDeltaT(double deltaM, double t)
        {
            return (1/growthRateK)*Math.Log(deltaM/(x0*Math.Pow(Math.E, growthRateK*t))+1);
        }
    }
}
