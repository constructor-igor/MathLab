using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using NLog;

namespace Ex1
{
    class Program
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        static void Main()
        {
            //SinglePopulationContinuousExperiment(new SinglePopulationDefinition { X0 = 1000, K = 0.1}, new ExperimentDefinition{M=1000000, DeltaM = 1});
            //SinglePopulationContinuousExperiment(new SinglePopulationDefinition { X0 = 1, K = 1}, new ExperimentDefinition { M = 10, DeltaM = 1 });

            //MultiSamePopulations();
            //MultiDifferentPopulations();

            MultiDependentPopulations();
        }

        private static void MultiDependentPopulations()
        {
            var experiment = new MultiDependentPopulationDiscreteExperiment();
            var populations = new List<SingleDependentPopulationDefinition>();
            
            var singleDependentPopulationDefinition1 = new SingleDependentPopulationDefinition(r:2, x0:1000);
            singleDependentPopulationDefinition1.Factors.Add(new DependentPopulationFactor(k:0));
            singleDependentPopulationDefinition1.Factors.Add(new DependentPopulationFactor(k:0.01));
            populations.Add(singleDependentPopulationDefinition1);

            var singleDependentPopulationDefinition2 = new SingleDependentPopulationDefinition(r: 2, x0: 1000);
            singleDependentPopulationDefinition2.Factors.Add(new DependentPopulationFactor(k: 0.1));
            singleDependentPopulationDefinition2.Factors.Add(new DependentPopulationFactor(k: 0));          
            populations.Add(singleDependentPopulationDefinition2);

            experiment.Run(populations, new ExperimentDefinition { M = 1000000, DeltaM = 1 });
        }

        private static void MultiSamePopulations()
        {
            var populations = SinglePopulationDefinition.New(3, X0: 1000, k: 0.1);
            MultiPopulationContinuousExperiment(populations, new ExperimentDefinition { M = 1000000, DeltaM = 1 });
        }

        private static void MultiDifferentPopulations()
        {
            var populations = new List<SinglePopulationDefinition>();
            populations.Add(new SinglePopulationDefinition {K = 2, X0 = 1000});
            populations.Add(new SinglePopulationDefinition {K = 0.1, X0 = 1000});
            populations.Add(new SinglePopulationDefinition {K = 10, X0 = 1000});
            MultiPopulationContinuousExperiment(populations, new ExperimentDefinition {M = 1000000, DeltaM = 1});
        }

        private static void SinglePopulationContinuousExperiment(SinglePopulationDefinition singlePopulationDefinition, ExperimentDefinition experimentDefinition)
        {            
            double X0 = singlePopulationDefinition.X0;
            double growthRateK = singlePopulationDefinition.K;

            double M = experimentDefinition.M;
            double deltaM = experimentDefinition.DeltaM;

            logger.Info("");
            logger.Info("M={0}, X0={1}, k={2}, deltaM={3}", M, X0, growthRateK, deltaM);

            var singleContinuousPopulation = new SingleContinuousPopulation(X0, growthRateK);
            double analyticalSolvedTime = singleContinuousPopulation.CalculateT(M);
            logger.Info("analytical calculated t = {0}, population = {1}", analyticalSolvedTime, singleContinuousPopulation.X(analyticalSolvedTime));

            double X = X0;
            double t = 0;
            int iterations = 0;
            while (Math.Abs(X - M) > deltaM)
            {
                t = t + singleContinuousPopulation.CalculateDeltaT(deltaM, t);
                X = singleContinuousPopulation.X(t);
                iterations++;
            }

            logger.Info("simulation calculated t = {0}, population = {1}, iterations = {2}", 
                t, singleContinuousPopulation.X(t), iterations);
        }

        private static void MultiPopulationContinuousExperiment(List<SinglePopulationDefinition> definitions, ExperimentDefinition experimentDefinition)
        {
            double M = experimentDefinition.M;
            double deltaM = experimentDefinition.DeltaM;

            logger.Info("");
            logger.Info("multi population experiment");
            logger.Info("M={0}, deltaM={1}", M, deltaM);

            var stringBuilder = new StringBuilder();
            for (int i = 0; i < definitions.Count; i++)
            {
                stringBuilder.AppendFormat("(X0{0} = {1}, K{0}={2}), ", i, definitions[i].X0, definitions[i].K);
            }
            logger.Info("initial populations state: {0}", stringBuilder);

            double singleDeltaM = deltaM/definitions.Count;
            var populations = definitions.Select(definition => new SingleContinuousPopulation(definition.X0, definition.K)).ToList();

            double X = definitions.Sum(d => d.X0);
            
            double t = 0;
            int iterations = 0;
            logger.Trace("#{0}, time={1}, X={2}", iterations, t, X);
            while (X<M && M - X > deltaM)
            {
                double actualDeltaM = Math.Max(deltaM, Math.Abs(M - X - deltaM));
                double actualSingleDeltaM = actualDeltaM / definitions.Count;

                //double deltaT = populations.Min(d => d.CalculateDeltaT(singleDeltaM, t));
                double deltaT = populations.Min(d => d.CalculateDeltaT(actualSingleDeltaM, t));
                t += deltaT;
                X = populations.Sum(d => d.X(t));
                iterations++;
                logger.Trace("#{0}, time={1}, X={2}", iterations, t, X);
            }

            logger.Info("simulation calculated t = {0}, populations = {1}, iterations = {2}",
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
