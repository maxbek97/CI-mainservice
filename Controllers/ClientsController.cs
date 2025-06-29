using CI_mainservice.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data;
using Dapper;
using CI_mainservice.Services;
using CI_mainservice.Settings;
using Microsoft.Extensions.Options;
using CI_mainservice.Repositories;

namespace CI_mainservice.Controllers;

[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{
    private readonly ConnectionStrings _settings;
    private readonly IOptions<HostMail> _settings_mail;
    public ClientsController(IOptions<ConnectionStrings> _options, IOptions<HostMail> _options_mail)
    {
        _settings = _options.Value;
        _settings_mail = _options_mail;
    }

    [HttpPost("submit")]
    public async Task<IActionResult> Submit(ClientRequest request)
    {
        MainService main_service = new MainService(_settings, _settings_mail, request);
        try
        {
            main_service.main_process();
            return Ok(new { message = "ok" });
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return Ok(new { message = e.Message });
        }
    }
}