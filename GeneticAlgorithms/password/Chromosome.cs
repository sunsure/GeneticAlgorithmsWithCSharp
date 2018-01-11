﻿using System;
using System.Diagnostics;
using System.Linq;

namespace GeneticAlgorithms.password
{
    public class Chromosome
    {
        public string Genes { get; }

        public int Fitness { get; }

        public Chromosome(string genes, int fitness)
        {
            Genes = genes;
            Fitness = fitness;
        }
    }
}