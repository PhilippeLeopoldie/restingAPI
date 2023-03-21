using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
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
  [Route("api/Presidents")]
  public class PresidentsController : ControllerBase
  {
    private IPresidentRepository _repo;

    public PresidentsController(IPresidentRepository repo)
    {
      _repo = repo;
    }

    [HttpGet]
    public ActionResult<IEnumerable<President>> GetPresidentList()
    {
      return Ok(_repo.GetAll());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<President> GetPresidentById(string id)
    {
      var president = _repo.GetOne(id);
      if (president == null || id == null) return NotFound();
      return Ok(president);
    }
    
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public  ActionResult<President> CreatePresident(AddPresidentRequest president )
    {
      if (president == null ) return BadRequest();
      if (string.IsNullOrEmpty(president.Name))return BadRequest("The Name field is required");
      if (string.IsNullOrEmpty(president.From)) return BadRequest("The From field is required");
      if (!Int16.TryParse(president.From, out var fromValue) || president.From.Length != 4 ) return BadRequest("The From field has a wrong date format: should be Year 'YYYY'");
      if (!string.IsNullOrEmpty(president.To) && !Int16.TryParse(president.To, out var toValue)) return BadRequest("The To field has a wrong date format: should be Year 'YYYY'");
      if (!string.IsNullOrEmpty(president.To) &&  president.To.Length != 4 ) return BadRequest("The To field has a wrong date format: should be Year 'YYYY'");
      var newPresident =  _repo.Create(president.Name,president.From,president.To);
      return CreatedAtAction(nameof(GetPresidentById), new { id = newPresident.Id }, newPresident);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<President> UpdatePresident(string id ,UpdatePresidentRequest president)
    {
      if (_repo.GetOne(id) == null) return NotFound();
      if (president == null ) return BadRequest();
      if (string.IsNullOrEmpty(president.Name)) return BadRequest("The Name field is required");
      if (string.IsNullOrEmpty(president.From)) return BadRequest("The From field is required");
      if (!Int16.TryParse(president.From, out var fromValue) || president.From.Length != 4 ) return BadRequest("The From field has a wrong date format: should be Year 'YYYY'");
      if (!string.IsNullOrEmpty(president.To) && !Int16.TryParse(president.To, out var toValue)) return BadRequest("The To field has a wrong date format: should be Year 'YYYY'");
      if (!string.IsNullOrEmpty(president.To) &&  president.To.Length != 4 ) return BadRequest("The To field has a wrong date format: should be Year 'YYYY'");
      return  Ok(_repo.Update(id,president.Name,president.From,president.To));
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(string id)
    {
      if (_repo.GetOne(id) == null) return NoContent();
      _repo.Delete(id);
      return NoContent();
    }
  }
}