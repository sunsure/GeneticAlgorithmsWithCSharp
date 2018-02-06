﻿/* File: CardTests.cs
 *     from chapter 6 of _Genetic Algorithms with Python_
 *     writen by Clinton Sheppard
 *
 * Author: Greg Eakin <gregory.eakin@gmail.com>
 * Copyright (c) 2018 Greg Eakin
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *   http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
 * implied.  See the License for the specific language governing
 * permissions and limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GeneticAlgorithms.Cards
{
    [TestClass]
    public class CardTests
    {
        private static readonly Random Random = new Random();

        public static Fitness GetFitness(int[] genes, int size)
        {
            var group1Sum = genes.Take(5).Sum();
            var group2Product = genes.Skip(5).Aggregate(1, (acc, val) => acc * val);
            var duplicateCount = genes.Length - new HashSet<int>(genes).Count;
            return new Fitness(group1Sum, group2Product, duplicateCount );
        }

        public static void Display(Chromosome<int, Fitness> candidate, Stopwatch watch)
        {
            Console.WriteLine("{0} - {1}\t{2},\t{3} ms",
                string.Join(", ", candidate.Genes.Take(5)),
                string.Join(", ", candidate.Genes.Skip(5)),
                candidate.Fitness, watch.ElapsedMilliseconds);
        }

        public static void Mutate(int[] genes, int[] geneSet, Genetic<int, Fitness> genetic)
        {
            if (genes.Length == new HashSet<int>(genes).Count)
            {
                var count = Random.Next(1, 4);
                while (count-- > 0)
                {
                    var randomSample = genetic.RandomSample(Enumerable.Range(0, genes.Length).ToArray(), 2);
                    var indexA = randomSample[0];
                    var indexB = randomSample[1];
                    var temp = genes[indexA];
                    genes[indexA] = genes[indexB];
                    genes[indexB] = temp;
                }
            }
            else
            {
                var indexA = Random.Next(genes.Length);
                var indexB = Random.Next(geneSet.Length);
                genes[indexA] = geneSet[indexB];
            }
        }

        [TestMethod]
        public void FitnessTest()
        {
            var genes = new[] {1, 1, 1, 1, 1, 2, 2, 2, 2, 2};
            var fitness = GetFitness(genes, genes.Length);
            Assert.AreEqual(5, fitness.Group1Sum);
            Assert.AreEqual(32, fitness.Group2Product);
            Assert.AreEqual(8, fitness.DuplicateCount);
        }

        [TestMethod]
        public void DispalyTest()
        {
            var genes = new[] {1, 1, 1, 1, 1, 2, 2, 2, 2, 2};
            var fitness = GetFitness(genes, genes.Length);
            var chromosome = new Chromosome<int, Fitness>(genes, fitness);
            var watch = Stopwatch.StartNew();

            Display(chromosome, watch);
        }

        [TestMethod]
        public void CardTest()
        {
            var genetic = new Genetic<int, Fitness>();
            var geneSet = Enumerable.Range(1, 10).ToArray();
            var watch = Stopwatch.StartNew();

            void FnDisplay(Chromosome<int, Fitness> candidate) => Display(candidate, watch);
            Fitness FnGetFitness(int[] genes) => GetFitness(genes, genes.Length);
            void FnMutate(int[] genes) => Mutate(genes, geneSet, genetic);

            var optimalFitness = new Fitness(36, 360, 0);
            var best = genetic.GetBest(FnGetFitness, 10, optimalFitness, geneSet, FnDisplay, FnMutate);
            Assert.IsTrue(optimalFitness.CompareTo(best.Fitness) <= 0);
        }

        [TestMethod]
        public void BenchmarkTest()
        {
            Benchmark.Run(CardTest);
        }
    }
}