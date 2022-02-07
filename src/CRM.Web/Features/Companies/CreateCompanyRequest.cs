using System;

namespace CRM.Web.Features.Companies;

public class CreateCompanyRequest
{
    public Guid CompanyId { get; set; }
    public string DomainName { get; set; }
}