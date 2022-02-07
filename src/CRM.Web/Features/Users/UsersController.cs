using System;
using System.Threading.Tasks;
using CRM.Web.Features.Companies;
using CRM.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace CRM.Web.Features.Users
{
    [ApiController]
    [Route("/companies/{companyId:guid}/users/{userId:guid}")]
    public class UsersController: ControllerBase
    {
        private readonly CrmContext _context;
        private readonly UsersRepository _usersRepository;
        private readonly CompanyRepository _companyRepository;
        private readonly EventDispatcher _eventDispatcher;

        public UsersController(CrmContext context, IMessageBus messageBus, IDomainLogger domainLogger)
        {
            _context = context;
            _eventDispatcher = new EventDispatcher(messageBus, domainLogger);
            _companyRepository = new CompanyRepository(context);
            _usersRepository = new UsersRepository(context);
        }

        /// <summary>
        /// Changes user email
        /// </summary>
        [HttpGet]
        [Route("email/{newEmail}")]
        public async Task<IActionResult> ChangeEmail(Guid companyId, Guid userId, string newEmail)
        {
            var user = await _usersRepository.GetUserById(userId);
            var company = await _companyRepository.GetCompany(companyId);
            if (company == null)
            {
                return BadRequest($"There is no company with id: {companyId}");
            }

            var canChangeEmailResult = user.CanChangeEmail();
            if (canChangeEmailResult.IsFailure)
            {
                return BadRequest(canChangeEmailResult.Messages);
            }
            
            user.ChangeEmail(newEmail, company);

            await _companyRepository.Save(company);
            await _usersRepository.Save(user);
            await _context.SaveChangesAsync();

            _eventDispatcher.Dispatch(user.PendingEvents);
            
            return Ok();
        }
    }
}