using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Resting.API.Data;

namespace Resting.API.Controllers
{

  public class President
  {
    [Key]
    [Required]
    public string Id { get; set; }
    public string Name { get; set; }
    public string From { get; set; }
    public string To { get; set; }
  }

  public class AddPresidentRequest
  {
    public string Name { get; set; }
    public string From { get; set; }
    public string To { get; set; }
  }

  public class UpdatePresidentRequest
  {
    public string Name { get; set; }
    public string From { get; set; }
    public string To { get; set; }
  }

  [ApiController]
  [Route("")]
  public class PresidentsController : ControllerBase
  {
    private IPresidentRepository _repo;
    
    public PresidentsController(IPresidentRepository repo) => _repo = repo;
  }
}