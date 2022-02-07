using System;

namespace CRM.Web.Features.Companies;

public class CompanyViewModel
{
    public Guid Id { get; set; }
    public string DomainName { get; set; }
    public int NumberOfEmployees { get; set; }
}