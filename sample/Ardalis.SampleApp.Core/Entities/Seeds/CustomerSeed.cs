using System.Collections.Generic;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;

namespace Ardalis.SampleApp.Core.Entities.Seeds
{
  public static class CustomerSeed
  {
    public static List<Customer> Seed()
    {
      List<Customer> output = new List<Customer>();

      for (int i = 1; i <= 100000; i++)
      {
        var customer = new Customer($"Customer{i}", $"Email{i}@local", $"Customer{i} address");
        customer.AddStore(new Store($"Store{i}-1", $"Store{i}-1 address"));
        customer.AddStore(new Store($"Store{i}-2", $"Store{i}-2 address"));

        output.Add(customer);
      }

      return output;
    }
  }
}
