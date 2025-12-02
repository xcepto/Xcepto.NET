using System;
using System.Collections.Generic;
using Xcepto.Data;

namespace Xcepto.Repositories;

public class CompartmentRepository
{
    private Dictionary<string, Compartment> _compartments = new();

    public Compartment GetCompartment(string key)
    {
        return _compartments[key];
    }

    public IEnumerable<Compartment> GetCompartments()
    {
        return _compartments.Values;
    }

    internal void AddCompartment(Compartment compartment)
    {
        if (_compartments.ContainsKey(compartment.UniqueName))
            throw new ArgumentException($"Compartment {compartment.UniqueName} not unique");
        _compartments.Add(compartment.UniqueName, compartment);
    }
}