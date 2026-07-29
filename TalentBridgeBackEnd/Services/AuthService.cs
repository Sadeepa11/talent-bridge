using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TalentBridgeBackEnd.Data;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.DTOs.Auth;

namespace TalentBridgeBackEnd.Services;

public class AuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ReferenceCodeGenerator _codeGenerator;

    public AuthService(AppDbContext context, IConfiguration configuration, ReferenceCodeGenerator codeGenerator)
    {
        _context = context;
        _configuration = configuration;
        _codeGenerator = codeGenerator;
    }

    public async Task<AuthResponseDto> RegisterCandidate(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Email already exists");

        var user = new User
        {
            Uuid = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, 12),
            Role = UserRole.Candidate,
            Status = UserStatus.Active,
            FailedLoginAttempts = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var candidateCode = await _codeGenerator.GenerateCandidateCode();
        var profile = new CandidateProfile
        {
            UserId = user.Id,
            ReferenceCode = candidateCode,
            PositionSought = "Pending",
            YearsExperience = 0,
            ExperienceBand = ExperienceBand.ZeroToOne,
            HighestQualification = "Pending",
            MainCity = "Pending",
            Availability = Availability.Immediate,
            Status = CandidateStatus.Draft,
            CompletenessPct = 10,
            LastActivityAt = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow
        };

        _context.CandidateProfiles.Add(profile);
        await _context.SaveChangesAsync();

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();
        
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = user.RefreshTokenExpiryTime.Value,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status
            }
        };
    }

    public async Task<AuthResponseDto> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if (user == null)
            throw new Exception("Invalid credentials");

        if (user.LockedUntil.HasValue && user.LockedUntil.Value > DateTime.UtcNow)
            throw new Exception("Account locked. Try again later.");

        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            user.FailedLoginAttempts++;
            if (user.FailedLoginAttempts >= 5)
            {
                user.LockedUntil = DateTime.UtcNow.AddMinutes(15);
            }
            await _context.SaveChangesAsync();
            throw new Exception("Invalid credentials");
        }

        user.FailedLoginAttempts = 0;
        user.LockedUntil = null;
        user.LastLoginAt = DateTime.UtcNow;

        var accessToken = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            RefreshTokenExpiration = user.RefreshTokenExpiryTime.Value,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status
            }
        };
    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RefreshToken))
            throw new Exception("Invalid refresh token");

        var user = await _context.Users.FirstOrDefaultAsync(u => u.RefreshToken == dto.RefreshToken);
        if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            throw new Exception("Refresh token is expired or invalid");

        var newAccessToken = GenerateJwtToken(user);
        var newRefreshToken = GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = newAccessToken,
            RefreshToken = newRefreshToken,
            RefreshTokenExpiration = user.RefreshTokenExpiryTime.Value,
            User = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status
            }
        };
    }

    public async Task<UserDto?> GetCurrentUser(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            Status = user.Status
        };
    }

    private string GenerateJwtToken(User user)
    {
        var secretKey = _configuration["JwtSettings:SecretKey"] ?? "TalentBridge-Super-Secret-Key-2026-Must-Be-At-Least-32-Characters-Long!";
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiryMinutesStr = _configuration["JwtSettings:ExpiryInMinutes"];
        var expiryMinutes = !string.IsNullOrEmpty(expiryMinutesStr) && int.TryParse(expiryMinutesStr, out var m) ? m : 120; // 2 hours default

        var claims = new[]
        {
            new Claim("userId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("companyId", user.CompanyId?.ToString() ?? "")
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtSettings:Issuer"] ?? "TalentBridge",
            audience: _configuration["JwtSettings:Audience"] ?? "TalentBridgeUsers",
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiryMinutes), // 2 hours
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
