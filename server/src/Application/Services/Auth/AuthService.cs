using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models.Entities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services.Auth
{
	public class AuthService : IAuthService
	{
		private readonly IConfiguration _configuration;

		public AuthService(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public string GenerateToken(User user)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
					new Claim(JwtRegisteredClaimNames.Email, user.Email),
					new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
					new Claim(ClaimTypes.Role, "User"), // Add roles if needed
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			var token = new JwtSecurityToken(
					issuer: _configuration["Jwt:Issuer"],
					audience: _configuration["Jwt:Audience"],
					claims: claims,
					expires: DateTime.UtcNow.AddHours(4),
					signingCredentials: credentials);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public bool ValidateToken(string token)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

			try
			{
				tokenHandler.ValidateToken(token, new TokenValidationParameters
						{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(key),
						ValidateIssuer = true,
						ValidIssuer = _configuration["Jwt:Issuer"],
						ValidateAudience = true,
						ValidAudience = _configuration["Jwt:Audience"],
						ValidateLifetime = true,
						ClockSkew = TimeSpan.Zero
						}, out _);

				return true;
			}
			catch
			{
				return false;
			}
		}

		public string HashPassword(string password)
		{
			using var hmac = new HMACSHA256();
			byte[] salt = hmac.Key;  // Generate a unique salt
			byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

			// Combine salt and hash into a single string
			return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
		}

		public bool VerifyPassword(string password, string storedHash)
		{
			var parts = storedHash.Split('.');
			if (parts.Length != 2) return false;

			byte[] salt = Convert.FromBase64String(parts[0]);
			byte[] storedHashBytes = Convert.FromBase64String(parts[1]);

			using var hmac = new HMACSHA256(salt);  // Use the stored salt
			byte[] computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

			return computedHash.SequenceEqual(storedHashBytes);
		}

	}
}

