using Microsoft.AspNetCore.Mvc;
using QuantityMeasurement.Application.Interfaces;
using QuantityMeasurement.Application.Models;

namespace QuantityMeasurement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HistoryController : ControllerBase
{
    private readonly IHistoryRepository _historyRepository;

    public HistoryController(IHistoryRepository historyRepository)
    {
        _historyRepository = historyRepository;
    }

    [HttpGet]
    public ActionResult<List<HistoryRecord>> Get()
    {
        var history = _historyRepository.GetHistory();
        return Ok(history);
    }

    [HttpDelete]
    public IActionResult Delete()
    {
        _historyRepository.ClearHistory();
        return NoContent();
    }
}