using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Resting.API.Controllers;

namespace Resting.API.Data
{
  public class SeedDatabase
  {
    public void PrepPopulation(IApplicationBuilder app)
    {
      using (var serviceScope = app.ApplicationServices.CreateScope())
      {
        SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>());
      }
    }
    private void SeedData(AppDbContext context)
    {
      if (!context.Presidents.Any())
      {
        addPresidents(context);
      }
      context.SaveChanges();
    }

    private void addPresidents(AppDbContext context)
    {
      Console.WriteLine("### Adding products to the empty database");

      context.Presidents.AddRange(
        new President
        {
          Id = "2f81a686-7531-11e8-86e5-f0d5bf731f68",
          From = "2001",
          To = "2009",
          Name = "George W. Bush"
        },
        new President
        {
          Id = "f9ce325d-ed8c-4fad-899b-fc997ed199ad",
          From = "2009",
          To = "2017",
          Name = "Barack Obama"
        },
        new President
        {
          Id = "b769d25a-86dc-4ec6-a022-dfa4112354f9",
          From = "2017",
          To = "2021",
          Name = "Donald Trump"
        },
        new President
        {
          Id = "822dcf18-54eb-4394-8884-1c73addf25c7",
          From = "2021",
          Name = "Joe Biden"
        }
      );
    }
  }
}