using System.Collections.Generic;
using Resting.API.Controllers;

namespace Resting.API.Data
{
  public interface IPresidentRepository
  {
    President GetOne(string id);
    IEnumerable<President> GetAll();
    President Create(string name, string from, string to);
    void Delete(string id);
    President Update(string id, string name, string from, string to);
  }
}