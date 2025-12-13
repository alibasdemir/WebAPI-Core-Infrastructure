using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        // We use the Service Locator pattern (via HttpContext) here instead of standard Constructor Injection.
        // Reason: To avoid forcing every derived controller to inject IMediator in their constructor just to pass it to the base class.
        // --- EXAMPLE OF WHAT WE AVOIDED (Without this approach) ---
        // If we used standard DI, every child controller would look like this:
        // public class TestsController : BaseController
        // {
        //     public TestsController(IMediator mediator) : base(mediator) // <--- Forced dependency passing
        //     {
        //     }
        // }
        // ----------------------------------------------------------
        // This design choice keeps the constructors of child controllers cleaner and reduces boilerplate code.
        protected IMediator _mediator => HttpContext.RequestServices.GetService<IMediator>() ?? throw new InvalidOperationException("IMediator can't be resolved.");
    }
}
