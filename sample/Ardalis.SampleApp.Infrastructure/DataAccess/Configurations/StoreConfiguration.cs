using System;
using System.Collections.Generic;
using System.Text;
using Ardalis.SampleApp.Core.Entities.CustomerAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ardalis.SampleApp.Infrastructure.DataAccess.Configurations;

public class StoreConfiguration : IEntityTypeConfiguration<Store>
{
  public void Configure(EntityTypeBuilder<Store> builder)
  {
    builder.ToTable(nameof(Store));

    builder.HasKey(x => x.Id);
  }
}
