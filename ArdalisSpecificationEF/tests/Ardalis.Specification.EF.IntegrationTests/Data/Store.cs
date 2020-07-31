﻿using System.Collections.Generic;

namespace Ardalis.Specification.EntityFrameworkCore.IntegrationTests.Data
{
    public class Store
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int CompanyId { get; set; }
        public Company? Company { get; set; }

        public int AddressId { get; set; }
        public Address? Address { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
