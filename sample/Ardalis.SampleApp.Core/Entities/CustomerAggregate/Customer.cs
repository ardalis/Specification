using System.Collections.Generic;
using System.Linq;
using Ardalis.GuardClauses;
using Ardalis.SampleApp.Core.Interfaces;

namespace Ardalis.SampleApp.Core.Entities.CustomerAggregate
{
    public class Customer : IAggregateRoot
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Address { get; private set; }

        public IEnumerable<Store> Stores => _stores.AsEnumerable();
        private readonly List<Store> _stores = new List<Store>();

        public Customer(string name, string email, string address)
        {
            Guard.Against.NullOrEmpty(name, nameof(name));
            Guard.Against.NullOrEmpty(email, nameof(email));

            this.Name = name;
            this.Email = email;
            this.Address = address;
        }

        public Store GetStore(int storeId)
        {
            var store = Stores.FirstOrDefault(x => x.Id == storeId);

            Guard.Against.Null(store, nameof(store));

            return store;
        }

        public Store AddStore(Store store)
        {
            Guard.Against.Null(store, nameof(store));

            // Do some other operation while adding it.

            _stores.Add(store);

            return store;
        }

        public void DeleteStore(int storeID)
        {
            var store = GetStore(storeID);

            _stores.Remove(store);
        }
    }
}
