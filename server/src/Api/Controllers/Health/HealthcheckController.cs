using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Health
{
	[Route("/api/[controller]")]
	[ApiController]
	public class HealthcheckController : ControllerBase
	{
		[HttpGet]
		public IActionResult HealthCheck() {
			return Ok("Shagadelic baby");
		}
	}
}
