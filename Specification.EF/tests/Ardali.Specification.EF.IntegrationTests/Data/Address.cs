using System;
using System.Collections.Generic;
using System.Text;

namespace Ardalis.Specification.EF.IntegrationTests.Data
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; }
    }
}
