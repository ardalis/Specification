---
layout: default
title: Quick Start Guide
parent: Getting Started
nav_order: 2
---

# Ardalis.Specification Quick Start Guide


1. Install Nuget-Package(s)
    a. Always required: [Ardalis.Specification](https://www.nuget.org/packages/Ardalis.Specification/)
    b. If you want to use it with EF Core also install the package [Ardalis.Specification.EntityFrameworkCore](https://www.nuget.org/packages/Ardalis.Specification.EntityFrameworkCore/)
2. Derive Repository from `RepositoryBase<T>`
    ```csharp
    public class YourRepository<T> : RepositoryBase<T> where T : class 
    {
        private readonly YourDbContext _dbContext;

        public YourRepository(YourDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }    
    }
    ```

3. Create a first specification
    ```csharp
    public class CustomerByLastnameSpec : Specification<Herstellerkontakt>
    {
        public CustomerByLastnameSpec(string lastname)
        {
            Query.Where(customer => c.Lastname == lastname);
        }
    }
    ```
    
4. Bind it all together:
    ```csharp
    public class CustomerService {
        private readonly customerRepository;

        public CustomerService (YourRepository<Customer> customerRepository) {
            this.customerRepository = customerRepository;
        }

        public Task<List<Customer>> GetCustomersByLastname(string lastname) {
            return customerRepository.ListAsync(new CustomerByLastnameSpec(lastname));
        }
    }
    ```
