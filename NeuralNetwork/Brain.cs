﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace NeuralNetwork
{
    public class Brain
    {
        public List<NeuronLayer> Layers { get; set; }

        public int NumberOfInputs => Layers.First().NumberOfInputs;

        public List<double> Genes
        {
            get => Layers.SelectMany(l => l.Weights).ToList();
            set
            {
                for (var i = 0; i < Layers.Count; i++)
                {
                    var layer = Layers[i];
                    for (var j = 0; j < layer.Neurons.Count; j++)
                    {
                        var neuron = layer.Neurons[j];
                        for (var k = 0; k < neuron.Weights.Count; k++)
                        {
                            neuron.Weights[k] = value[i + j + k];
                        }
                    }
                }
            }
        }

        public Brain(int numInputs, int numHiddenLayers, int layerWidth, int numOutputs, double p, Random rand = null)
        {
            if (numHiddenLayers < 1)
            {
                throw new ArgumentException("Must have at least 1 hidden layer.", nameof(numHiddenLayers));
            }
            var random = rand ?? new Random();
            Layers = new List<NeuronLayer>();
            Layers.Add(new NeuronLayer(numInputs, layerWidth, p, random));
            Layers.AddRange(new int[numHiddenLayers - 1].Select(i => new NeuronLayer(layerWidth, layerWidth, p, random)).ToList());
            Layers.Add(new NeuronLayer(layerWidth, numOutputs, p, random));
        }

        public List<double> Think(List<double> inputs)
        {
            var previousOutput = inputs;
            foreach (var layer in Layers)
            {
                previousOutput = layer.Think(previousOutput);
            }
            return previousOutput;
        }

        public async Task<List<double>> ThinkAsync(List<double> inputs)
        {
            var previousOutput = inputs;
            foreach (var layer in Layers)
            {
                previousOutput = await layer.ThinkAsync(previousOutput);
            }
            return previousOutput;
        }
    }
}
