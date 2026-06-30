using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.OrderContactAPC;
using System.Collections.Generic;

namespace ServiceHost.Areas.Admin.Pages.Panel.OrderContacts
{
    public class IndexModel : PageModel
    {
        public List<OrderContactViewModel> ContactList { get; set; }
        private readonly IOrderContactApplication _orderContactApplication;

        public IndexModel(IOrderContactApplication orderContactApplication)
        {
            _orderContactApplication = orderContactApplication;
        }

        public void OnGet()
        {
            ContactList = _orderContactApplication.GetList();
        }

        public IActionResult OnGetMarkAsRead(long id)
        {
            _orderContactApplication.MarkAsRead(id);
            return RedirectToPage("./Index");
        }
    }
}