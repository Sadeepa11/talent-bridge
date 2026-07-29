using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;

namespace TalentBridgeBackEnd.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context, string connectionString)
        {
            try
            {
                // 1. Ensure the MySQL database schema 'talentbridge' is created on MySQL server
                var connBuilder = new MySqlConnectionStringBuilder(connectionString);
                var databaseName = connBuilder.Database;

                connBuilder.Database = ""; // Connect to server without specific database
                using (var masterConn = new MySqlConnection(connBuilder.ConnectionString))
                {
                    await masterConn.OpenAsync();
                    using var cmd = masterConn.CreateCommand();
                    cmd.CommandText = $"CREATE DATABASE IF NOT EXISTS `{databaseName}` CHARACTER SET utf8mb4;";
                    await cmd.ExecuteNonQueryAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DbInitializer] Database creation check note: {ex.Message}");
            }

            // 2. Ensures all 19 tables are created in MySQL database
            await context.Database.EnsureCreatedAsync();

            // 3. Seed default super admin user if empty
            if (!await context.Users.AnyAsync(u => u.Role == UserRole.SuperAdmin))
            {
                var adminUser = new User
                {
                    Uuid = Guid.NewGuid(),
                    Email = "admin@talentbridge.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin.123", 12),
                    Role = UserRole.SuperAdmin,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                var opsUser = new User
                {
                    Uuid = Guid.NewGuid(),
                    Email = "ops@talentbridge.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Ops.123", 12),
                    Role = UserRole.OpsAdmin,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.AddRange(adminUser, opsUser);
                await context.SaveChangesAsync();
            }

            // Seed default demo company if empty
            if (!await context.Companies.AnyAsync())
            {
                var company = new Company
                {
                    Name = "TechNova Solutions Ltd",
                    BusinessRegNo = "PV-12345",
                    Industry = "Software Development",
                    Address = "100 Galle Road, Colombo 03",
                    ContactPerson = "John Doe",
                    ContactEmail = "contact@technova.com",
                    ContactPhone = "+94112345678",
                    Status = CompanyStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                context.Companies.Add(company);
                await context.SaveChangesAsync();

                var companyUser = new User
                {
                    Uuid = Guid.NewGuid(),
                    Email = "company@test.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Company.123", 12),
                    Role = UserRole.CompanyUser,
                    CompanyId = company.Id,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(companyUser);
                await context.SaveChangesAsync();
            }

            // Seed default candidate if empty
            if (!await context.CandidateProfiles.AnyAsync())
            {
                var candidateUser = new User
                {
                    Uuid = Guid.NewGuid(),
                    Email = "candidate@talentbridge.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("Candidate.123", 12),
                    Role = UserRole.Candidate,
                    Status = UserStatus.Active,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(candidateUser);
                await context.SaveChangesAsync();

                var candidateProfile = new CandidateProfile
                {
                    UserId = candidateUser.Id,
                    ReferenceCode = "CND-2026-0001",
                    PositionSought = "Senior Full Stack Developer",
                    YearsExperience = 5,
                    ExperienceBand = ExperienceBand.FiveToTen,
                    HighestQualification = "BSc in Computer Science",
                    MainCity = "Colombo",
                    Availability = Availability.Immediate,
                    ExpectedSalaryMin = 250000,
                    ExpectedSalaryMax = 350000,
                    Status = CandidateStatus.Published,
                    CompletenessPct = 100,
                    PublishedAt = DateTime.UtcNow,
                    LastActivityAt = DateTime.UtcNow,
                    CreatedAt = DateTime.UtcNow
                };

                context.CandidateProfiles.Add(candidateProfile);
                await context.SaveChangesAsync();

                var candidatePii = new CandidatePii
                {
                    CandidateProfileId = candidateProfile.Id,
                    FullName = "Sunil Perera",
                    NicNumber = "199512345678",
                    Email = "candidate@talentbridge.com",
                    Mobile = "+94771234567",
                    AddressLine1 = "No 45, Main Street",
                    AddressLine2 = "Colombo 07",
                    PostalCode = "00700",
                    DateOfBirth = new DateTime(1995, 5, 15),
                    CreatedAt = DateTime.UtcNow
                };

                context.CandidatePiis.Add(candidatePii);

                var exp1 = new CandidateExperience
                {
                    CandidateProfileId = candidateProfile.Id,
                    EmployerName = "Virtusa Corporation",
                    EmployerDescriptor = "Tier-1 IT Services Provider",
                    JobTitle = "Senior Software Engineer",
                    Industry = "IT Services",
                    StartDate = new DateTime(2021, 1, 1),
                    Responsibilities = "Developed enterprise cloud web applications using .NET and React.",
                    CreatedAt = DateTime.UtcNow
                };

                context.CandidateExperiences.Add(exp1);

                var qual1 = new CandidateQualification
                {
                    CandidateProfileId = candidateProfile.Id,
                    QualificationName = "BSc (Hons) in Computing",
                    InstitutionName = "University of Moratuwa",
                    InstitutionDescriptor = "State University Sri Lanka",
                    Level = QualificationLevel.Bachelors,
                    YearCompleted = 2020,
                    CreatedAt = DateTime.UtcNow
                };

                context.CandidateQualifications.Add(qual1);

                await context.SaveChangesAsync();
            }
        }
    }
}
