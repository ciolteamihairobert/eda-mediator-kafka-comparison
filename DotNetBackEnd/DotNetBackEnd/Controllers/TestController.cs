using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("ping")]
    public async Task<string> Ping()
    {
        return await _mediator.Send(new PingCommand());
    }

    [HttpPost("send")]
    public async Task<IActionResult> Send([FromServices] KafkaProducer producer)
    {
        await producer.ProduceAsync("test-topic", "Hello Kafka!");
        return Ok("Message sent");
    }

}
