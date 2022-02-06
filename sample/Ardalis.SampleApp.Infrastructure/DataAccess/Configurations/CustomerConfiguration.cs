using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ardalis.SampleApp.Infrastructure.DataAccess.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
  public void Configure(EntityTypeBuilder<Customer> builder)
  {
    builder.ToTable(nameof(Customer));

    builder.Metadata.FindNavigation(nameof(Customer.Stores))
                    .SetPropertyAccessMode(PropertyAccessMode.Field);

    builder.HasKey(x => x.Id);
  }
}
