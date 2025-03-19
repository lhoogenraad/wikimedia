using Microsoft.AspNetCore.Mvc;
using Models.Entities;
using Infrastructure.Database;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly MongoDbContext _dbContext;

		public UserController(MongoDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		// GET: api/user
		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetUsers()
		{
			var users = await _dbContext.GetCollection<User>("Users").Find(_ => true).ToListAsync();
			return Ok(users);
		}

		// GET: api/user/{id}
		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetUser(string id)
		{
			var user = await _dbContext.GetCollection<User>("Users")
				.Find(u => u.Id == new MongoDB.Bson.ObjectId(id))
				.FirstOrDefaultAsync();

			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}

		// POST: api/user
		[HttpPost]
		public async Task<ActionResult<User>> CreateUser([FromBody] User user)
		{
			if (user == null)
			{
				return BadRequest("User data is invalid.");
			}

			user.CreatedDate = DateTime.UtcNow; // Setting created date
			user.IsActive = true; // Set default value for new users

			await _dbContext.GetCollection<User>("Users").InsertOneAsync(user);

			return CreatedAtAction(nameof(GetUser), new { id = user.Id.ToString() }, user);
		}

		// PUT: api/user/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult> UpdateUser(string id, [FromBody] User updatedUser)
		{
			if (updatedUser == null)
			{
				return BadRequest("User data is invalid.");
			}

			var existingUser = await _dbContext.GetCollection<User>("Users")
				.Find(u => u.Id == new MongoDB.Bson.ObjectId(id))
				.FirstOrDefaultAsync();

			if (existingUser == null)
			{
				return NotFound();
			}

			// Update the user data here
			existingUser.FirstName = updatedUser.FirstName;
			existingUser.LastName = updatedUser.LastName;
			existingUser.Email = updatedUser.Email;
			existingUser.IsActive = updatedUser.IsActive;

			await _dbContext.GetCollection<User>("Users")
				.ReplaceOneAsync(u => u.Id == new MongoDB.Bson.ObjectId(id), existingUser);

			return NoContent();
		}
	}
}

