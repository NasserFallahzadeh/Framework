using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public IActionResult Get()
    {
        FirstName firstName01 = "Alireza";
        FirstName firstName02 = "Alireza";
        return Ok(firstName01 == firstName02);
    }
}