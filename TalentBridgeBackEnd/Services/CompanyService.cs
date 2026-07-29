using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.Data;
using Microsoft.EntityFrameworkCore;

namespace TalentBridgeBackEnd.Services
{
    public class CompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Company>> GetCompaniesAsync()
        {
            return await _context.Companies.ToListAsync();
        }

        public async Task<Company?> GetCompanyByIdAsync(int id)
        {
            return await _context.Companies.FindAsync(id);
        }

        public async Task<Company> CreateCompanyAsync(Company company)
        {
            company.CreatedAt = DateTime.UtcNow;
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> UpdateCompanyAsync(int id, Company updatedData)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) throw new Exception("Company not found");

            company.Name = updatedData.Name;
            company.Industry = updatedData.Industry;
            company.Address = updatedData.Address;
            company.ContactPerson = updatedData.ContactPerson;
            company.ContactEmail = updatedData.ContactEmail;
            company.ContactPhone = updatedData.ContactPhone;
            company.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<Company> UpdateCompanyStatusAsync(int id, CompanyStatus status)
        {
            var company = await _context.Companies.FindAsync(id);
            if (company == null) throw new Exception("Company not found");

            company.Status = status;
            company.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return company;
        }

        public async Task<User> CreateCompanyUserAsync(int companyId, string email, string tempPassword)
        {
            var company = await _context.Companies.FindAsync(companyId);
            if (company == null) throw new Exception("Company not found");

            var user = new User
            {
                Uuid = Guid.NewGuid(),
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(tempPassword, 12),
                Role = UserRole.CompanyUser,
                CompanyId = companyId,
                Status = UserStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
