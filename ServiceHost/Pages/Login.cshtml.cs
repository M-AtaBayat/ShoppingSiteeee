using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.AccountAPC;
using System.Threading.Tasks;

namespace ServiceHost.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IAccountApplication _accountApplication;

        public LoginModel(IAccountApplication accountApplication)
        {
            _accountApplication = accountApplication;
        }

        [BindProperty]
        public LoginViewModel Command { get; set; }

        [BindProperty]
        public VerifyViewModel VerifyCommand { get; set; }

        public void OnGet() { }

        public JsonResult OnPostSendCode()
        {
            var result = _accountApplication.Login(Command);
            return new JsonResult(result);
        }

        public async Task<JsonResult> OnPostVerifyCode()
        {
            var result = await _accountApplication.Verify(VerifyCommand);
            return new JsonResult(result);
        }
    }
}