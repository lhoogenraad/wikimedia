using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Health
{
	[Route("/api/[controller]")]
	[ApiController]
	public class HealthcheckController : ControllerBase
	{
		[HttpGet]
		public async Task<ActionResult<String>> HealthCheck() {
			return Ok("Shagadelic baby");
		}
	}
}
