using System;
using System.Collections.Generic;
using System.Linq;

namespace NaiveBayes
{
    public static class MathStatistics
    {
        public static MeanVariance CalculateMeanVariance(this IList<double> data)
        {            
            int count = data.Count();
            double mean = data.Sum() / count;
            double variance = data.Sum(t => Math.Pow((t - mean), 2)) / (count - 1);

            var result = new MeanVariance {Mean = mean, Variance = variance};
            return result;
        }

        public static double CalcP(MeanVariance featureTrained, double examFeatureValue)
        {
            double varianceX2 = 2*featureTrained.Variance*featureTrained.Variance;
            double result = 1/(Math.Sqrt(Math.PI*varianceX2));
            double examMean = examFeatureValue - featureTrained.Mean;
            double examMeanX2 = examMean*examMean;
            double underExp = -examMeanX2 / varianceX2;
            result = result*Math.Exp(underExp);
            return result;
        }
    }

    public struct MeanVariance
    {
        public double Mean;
        public double Variance;
    }

}