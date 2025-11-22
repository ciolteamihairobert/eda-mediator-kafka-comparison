using DotNetBackEnd.Application.Orders.Commands;
using DotNetBackEnd.Application.Orders.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotNetBackEnd.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderCommand command, CancellationToken ct)
    {
        var id = await _mediator.Send(command, ct);
        return AcceptedAtAction(nameof(GetById), new { id }, new { OrderId = id });
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id), ct);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetAllOrdersQuery(), ct);
        return Ok(result);
    }
}
