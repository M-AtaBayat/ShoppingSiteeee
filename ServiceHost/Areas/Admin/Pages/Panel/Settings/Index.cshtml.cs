using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.SettingsAPC;

namespace ServiceHost.Areas.Admin.Pages.Panel.Settings
{
    public class IndexModel : PageModel
    {
        [BindProperty] public EditSettings Command { get; set; }
        public SettingsViewModel CurrentSettings { get; set; }

        private readonly ISettingsApplication _settingsApplication;

        public IndexModel(ISettingsApplication settingsApplication)
        {
            _settingsApplication = settingsApplication;
        }

        public void OnGet()
        {
            CurrentSettings = _settingsApplication.GetSettings();
            Command = new EditSettings
            {
                Id = CurrentSettings.Id,
                ShippingCost = CurrentSettings.ShippingCost,
                AdminEmail = CurrentSettings.AdminEmail
            };
        }

        public IActionResult OnPost()
        {
            var result = _settingsApplication.Edit(Command);
            return RedirectToPage("./Index");
        }
    }
}