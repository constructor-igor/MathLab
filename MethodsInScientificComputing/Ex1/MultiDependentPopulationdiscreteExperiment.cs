using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NLog;

namespace Ex1
{
    public class MultiDependentPopulationDiscreteExperiment
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public void Run(List<SingleDependentPopulationDefinition> definitions, ExperimentDefinition experimentDefinition)
        {
            Trace.Assert(definitions.Count == 2, "supported 2 populations only");
            double M = experimentDefinition.M;

            logger.Info("");
            logger.Info("multi dependent population experiment");
            logger.Info("M={0}", M);

            double X = definitions.Sum(d => d.X0);
            double X_previous = X;
            int iterations = 0;
            List<DependentPopulation> populations = definitions.Select(definition => new DependentPopulation(definition.X0)).ToList();            

            logger.Trace("#{0}, X={1}", iterations, X);
            while (X < M)
            {
                X_previous = X;
                double X1j = definitions[0].R*populations[0].Xj - definitions[0].Factors[1].K*populations[1].Xj;
                double X2j = definitions[1].R*populations[1].Xj - definitions[1].Factors[0].K*populations[0].Xj;
                double Xj = X1j + X2j;
                 
                X = Xj;                

                populations[0].Xj = X1j;
                populations[0].J++;
                populations[1].Xj = X2j;
                populations[1].J++;

                iterations++;
                logger.Trace("#{0}, X={1}", iterations, X);
            }

            logger.Info("simulation calculated , [answer:] populations = {0}, iterations = {1}", X_previous, iterations - 1);
            logger.Info("simulation calculated , [next:] populations = {0}, iterations = {1}", X, iterations);
        }
    }

    public class SingleDependentPopulationDefinition
    {
        public readonly int R;
        public readonly int X0;
        public readonly List<DependentPopulationFactor> Factors = new List<DependentPopulationFactor>();

        public SingleDependentPopulationDefinition(int r, int x0)
        {
            R = r;
            X0 = x0;
        }
    }

    public class DependentPopulationFactor
    {
        public readonly double K;

        public DependentPopulationFactor(double k)
        {
            K = k;
        }
    }

    public class DependentPopulation
    {
        public double Xj;
        public int J;

        public DependentPopulation(double X0)
        {
            J = 0;
            Xj = X0;
        }
    }
}