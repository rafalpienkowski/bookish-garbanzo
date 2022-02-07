using System;
using System.Threading.Tasks;
using CRM.Domain.Companies;
using CRM.Domain.Core;
using CRM.Web.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace CRM.Web.Features.Companies
{
    public class CompanyRepository
    {
        private readonly CrmContext _context;

        public CompanyRepository(CrmContext context)
        {
            _context = context;
        }

        public Task<Company> GetCompany(Guid id) => _context.Companies.FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Result> Save(Company company)
        {
            var isOtherCompanyWithTheSameDomainName = await _context.Companies.AnyAsync(c => c.Id != company.Id && c.DomainName == company.DomainName);
            if(isOtherCompanyWithTheSameDomainName)
            {
                return Result.Failure("There is another company with the same domain name");
            }
            
            var entity = await _context.Companies.SingleOrDefaultAsync(c => c.Id == company.Id);
            if (entity == null)
            {
                _context.Companies.Add(company);
            }
            else
            {
                _context.Entry(entity).CurrentValues.SetValues(company);
            }
            
            return Result.Successful();
        }
    }
}