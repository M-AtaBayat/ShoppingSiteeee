using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace ServiceHost.Areas.Admin.Pages
{
    [Authorize(Roles = "Admin")]
    public class AdminPageModel : PageModel
    {
    }
}