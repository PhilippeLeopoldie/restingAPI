using System;
using System.Collections.Generic;
using System.Linq;
using Resting.API.Controllers;

namespace Resting.API.Data
{
  public class PresidentRepository : IPresidentRepository
  {
    private readonly AppDbContext _context;

    public PresidentRepository(AppDbContext context) => _context = context;
    public President Create(string name, string from, string to)
    {
      var president = new President
      {
        Id = Guid.NewGuid().ToString(),
        Name = name,
        From = from,
        To = to
      };

      _context.Presidents.Add(president);
      _context.SaveChanges();
      return president;
    }


    public void Delete(string id)
    {
      var president = GetOne(id);
      if (president == null)
        return;

      _context.Presidents.Remove(president);
      _context.SaveChanges();
    }

    public IEnumerable<President> GetAll() => _context.Presidents.ToList();

    public President GetOne(string id) =>
      _context.Presidents.Where(c => c.Id == id)
        .SingleOrDefault();

    public President Update(string id, string name, string from, string to)
    {
      var president = GetOne(id);

      president.Name = name;
      president.To = to;
      president.From = from;

      var updatedPresident = _context.Presidents.Update(president);
      _context.SaveChanges();
      return updatedPresident.Entity;
    }

  }
}
