using Ardalis.GuardClauses;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.SampleApp.Core.Entitites.CustomerAggregate
{
    public class Store
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Address { get; private set; }

        public int CustomerId { get; private set; }

        public Store(string name, string address)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));

            this.Name = name;
            this.Address = address;
        }
    }
}
