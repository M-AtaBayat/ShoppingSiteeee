using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;
using System.Collections.Generic;

namespace ServiceHost.Areas.Admin.Pages.Panel.ManageOrder
{
    public class SentModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public SearchOrderModel SearchModel { get; set; }
        public List<OrderViewModel> Orders { get; set; }

        private readonly IOrderApplication _orderApplication;

        public SentModel(IOrderApplication orderApplication)
        {
            _orderApplication = orderApplication;
        }

        public void OnGet()
        {
            SearchModel.Status = 2;
            Orders = _orderApplication.Search(SearchModel);
        }

        public IActionResult OnGetDeliver(long id)
        {
            var result = _orderApplication.DeliverOrder(id);
            return RedirectToPage("./Sent");
        }
    }
}