using _0_Framework.Application;

namespace ShoppingSiteManagement.Application.Contracts.AccountAPC
{
    public interface IAccountApplication
    {
        OperationResult Login(LoginViewModel command);
        Task<OperationResult> Verify(VerifyViewModel command);
        OperationResult Logout();
        AccountViewModel GetLoggedInAccount(string email);
    }
}
