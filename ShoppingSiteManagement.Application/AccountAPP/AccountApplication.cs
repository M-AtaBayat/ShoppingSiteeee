using _0_Framework.Application;
using ShoppingSiteManagement.Application.Contracts.AccountAPC;
using ShoppingSiteManagement.Domain.AccountAgg;
using ShoppingSiteManagement.Infrastructure.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.AccountAPP
{
    public class AccountApplication : IAccountApplication
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IEmailService _emailService;
        private readonly IAuthHelper _authHelper;

        public AccountApplication(IAccountRepository accountRepository, IEmailService emailService, IAuthHelper authHelper)
        {
            _accountRepository = accountRepository;
            _emailService = emailService;
            _authHelper = authHelper;
        }

        public OperationResult Login(LoginViewModel command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetByEmail(command.Email);

            if (account == null)
            {
                int roleId = (command.Email == "bayatata88@gmail.com") ? 1 : 2;
                account = new Account(command.Email, roleId);

                account.GenerateVerificationCode();

                _accountRepository.Add(account);
                _accountRepository.Save();
            }
            else
            {
                account.GenerateVerificationCode();
                _accountRepository.Save();
            }

            _emailService.Send("کد تایید ورود به پارسو شاپ", $"کد تایید شما: {account.VerificationCode}", account.Email);
            return operation.Success("کد تایید به ایمیل شما ارسال شد.");
        }

        public async Task<OperationResult> Verify(VerifyViewModel command)
        {
            var operation = new OperationResult();
            var account = _accountRepository.GetByEmail(command.Email);

            if (account == null) return operation.Failed("حساب یافت نشد.");

            if (account.VerificationCode != command.Code)
                return operation.Failed("کد تایید اشتباه است.");

            var authModel = new AuthViewModel(account.Id, account.RoleId, account.Email);

            await _authHelper.Signin(authModel);

            return operation.Success("با موفقیت وارد شدید.");
        }

        public OperationResult Logout()
        {
            _authHelper.Signout();
            return new OperationResult().Success();
        }

        public AccountViewModel GetLoggedInAccount(string email)
        {
            var account = _accountRepository.GetByEmail(email);
            if (account == null) return null;

            return new AccountViewModel { Id = account.Id, Email = account.Email, IsActive = account.IsActive };
        }
    }
}