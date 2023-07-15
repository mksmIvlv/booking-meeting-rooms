using Application.Mediatr.Features.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController: ControllerBase
{
    #region Поля
    
    

    #endregion

    #region Конструктор

    public TestController()
    {
        
    }

    #endregion

    #region Api - методы

    [HttpGet]
    public async Task<IActionResult> TestMethod()
    {
        return Ok("7c0a2d75-b56b-4473-9cc5-e76e9591cc6d");
    }

    #endregion
}