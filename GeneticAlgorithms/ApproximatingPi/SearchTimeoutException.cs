﻿using System;

namespace GeneticAlgorithms.ApproximatingPi
{
    public partial class Genetic<TGene, TFitness>
        where TFitness : IComparable<TFitness>
    {
        public class SearchTimeoutException : Exception
        {
            public Chromosome<TGene, TFitness> Improvement { get; }

            public SearchTimeoutException(Chromosome<TGene, TFitness> improvement)
            {
                Improvement = improvement;
            }
        }
    }
}