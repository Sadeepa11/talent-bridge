using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TalentBridgeBackEnd.Models;
using TalentBridgeBackEnd.Models.Enums;
using TalentBridgeBackEnd.Services;
using TalentBridgeBackEnd.DTOs.Admin;

namespace TalentBridgeBackEnd.Controllers.Admin
{
    [ApiController]
    [Route("api/v1/admin/companies")]
    [Authorize(Roles = "SuperAdmin,OpsAdmin")]
    public class CompanyManagementController : ControllerBase
    {
        private readonly CompanyService _companyService;

        public CompanyManagementController(CompanyService companyService)
        {
            _companyService = companyService;
        }

        [HttpGet]
        public async Task<IActionResult> ListCompanies()
        {
            var companies = await _companyService.GetCompaniesAsync();
            return Ok(companies);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyCreateDto request)
        {
            var company = new TalentBridgeBackEnd.Models.Company
            {
                Name = request.Name,
                BusinessRegNo = request.BusinessRegNo,
                Industry = request.Industry,
                Address = request.Address,
                ContactPerson = request.ContactPerson,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone,
                Status = CompanyStatus.Active
            };

            var created = await _companyService.CreateCompanyAsync(company);
            return CreatedAtAction(nameof(GetCompanyDetail), new { id = created.Id }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCompanyDetail(int id)
        {
            var company = await _companyService.GetCompanyByIdAsync(id);
            if (company == null) return NotFound();
            return Ok(company);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] CompanyCreateDto request)
        {
            var company = new TalentBridgeBackEnd.Models.Company
            {
                Name = request.Name,
                Industry = request.Industry,
                Address = request.Address,
                ContactPerson = request.ContactPerson,
                ContactEmail = request.ContactEmail,
                ContactPhone = request.ContactPhone
            };

            var updated = await _companyService.UpdateCompanyAsync(id, company);
            return Ok(updated);
        }

        public class CompanyStatusUpdateDto
        {
            public CompanyStatus Status { get; set; }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateCompanyStatus(int id, [FromBody] CompanyStatusUpdateDto statusRequest)
        {
            var updated = await _companyService.UpdateCompanyStatusAsync(id, statusRequest.Status);
            return Ok(updated);
        }

        [HttpPost("{id}/users")]
        public async Task<IActionResult> CreateCompanyUser(int id, [FromBody] CompanyUserCreateDto request)
        {
            var user = await _companyService.CreateCompanyUserAsync(id, request.Email, "Temp.1234");
            return Ok(new { id = user.Id, email = user.Email, companyId = user.CompanyId });
        }
    }
}
