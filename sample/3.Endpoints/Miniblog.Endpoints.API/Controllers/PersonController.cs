using Microsoft.AspNetCore.Mvc;
using Miniblog.Core.Domain.People.Entities;
using Miniblog.Core.Domain.People.ValueObjects;

namespace Miniblog.Endpoints.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PersonController : ControllerBase
{
    private readonly ILogger<PersonController> _logger;
    public PersonController(ILogger<PersonController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetValueObjectEQ")]
    public IActionResult Get()
    {
        FirstName firstName01 = "Alireza1";
        FirstName firstName02 = "Alireza";
        return Ok(firstName01 == firstName02);
    }

    [HttpGet("/GetLenException")]
    public IActionResult GetLenException()
    {
        try
        {
            var firstName = new FirstName("a");
            return Ok("Ok");
        }
        catch (Exception e)
        {
            return Ok(e.ToString());
        }
    }

    [HttpGet("/CreatePerson")]
    public IActionResult CreatePerson()
    {
        try
        {
            var p = new Person(-1, "Alireza", "Oroumand");
            return Ok("Ok");
        }
        catch (Exception e)
        {
            return Ok(e.ToString());
        }
    }
}