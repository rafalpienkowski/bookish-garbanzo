using System;
using System.Linq;
using System.Threading.Tasks;
using CRM.Domain.Companies;
using CRM.Web.Features.Users;
using CRM.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CRM.Web.Features.Companies;

[ApiController]
[Route("/companies/")]
public class CompanyController : ControllerBase
{
    private readonly CompanyRepository _companyRepository;
    private readonly CrmContext _crmContext;
    private readonly EventDispatcher _eventDispatcher;

    public CompanyController(CrmContext crmContext, IMessageBus messageBus, IDomainLogger domainLogger)
    {
        _crmContext = crmContext;
        _companyRepository = new CompanyRepository(crmContext);
        _eventDispatcher = new EventDispatcher(messageBus, domainLogger);
    }

    /// <summary>
    /// Gets list of all active companies
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var companies = await _crmContext.Companies.Where(c => c.Active).Select(c => new CompanyViewModel
        {
            NumberOfEmployees = c.NumberOfEmployees,
            DomainName = c.DomainName,
            Id = c.Id
        }).ToListAsync();

        return Ok(companies);
    }

    /// <summary>
    /// Creates new company
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompanyRequest request)
    {
        var companyWithGivenId = await _companyRepository.GetCompany(request.CompanyId);
        if (companyWithGivenId != null)
        {
            return BadRequest($"There is already company with given id: {request.CompanyId}");
        }

        var company = Company.CreateNew(request.CompanyId, request.DomainName);

        var result = await _companyRepository.Save(company);
        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }

        await _crmContext.SaveChangesAsync();
        _eventDispatcher.Dispatch(company.PendingEvents);

        return Ok();
    }

    /// <summary>
    /// Closes a company
    /// </summary>
    [HttpDelete]
    [Route("{companyId:guid}")]
    public async Task<IActionResult> Close(Guid companyId)
    {
        var company = await _companyRepository.GetCompany(companyId);
        if (company == null)
        {
            return NoContent();
        }

        var canBeClosedResult = company.CanBeClosed();
        if (canBeClosedResult.IsFailure)
        {
            return BadRequest(canBeClosedResult.Messages);
        }
            
        company.Close();

        var result = await _companyRepository.Save(company);
        if (result.IsFailure)
        {
            return BadRequest(result.Messages);
        }

        await _crmContext.SaveChangesAsync();
        _eventDispatcher.Dispatch(company.PendingEvents);

        return NoContent();
    }
}