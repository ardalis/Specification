using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Ardalis.SampleApp.Core.Entities.Seeds;
using Microsoft.EntityFrameworkCore;

namespace Ardalis.SampleApp.Infrastructure.DataAccess;

public class SampleDbContextSeed
{
  private readonly SampleDbContext dbContext;

  public SampleDbContextSeed(SampleDbContext dbContext)
  {
    this.dbContext = dbContext;
  }

  public async Task SeedAsync(int retry = 0)
  {
    try
    {
      dbContext.Database.Migrate();

      if (await dbContext.Customers.CountAsync() == 0)
      {
        foreach (var customer in CustomerSeed.Seed())
        {
          dbContext.Customers.Add(customer);
        }
      }

      await dbContext.SaveChangesAsync();

    }
    catch (Exception)
    {
      if (retry > 0)
      {
        await SeedAsync(retry - 1);
      }
    }
  }
}
