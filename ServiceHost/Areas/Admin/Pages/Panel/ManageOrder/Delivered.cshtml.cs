using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;
using System.Collections.Generic;

namespace ServiceHost.Areas.Admin.Pages.Panel.ManageOrder
{
    public class DeliveredModel : PageModel
    {
        [BindProperty(SupportsGet = true)] public SearchOrderModel SearchModel { get; set; }
        public List<OrderViewModel> Orders { get; set; }

        private readonly IOrderApplication _orderApplication;

        public DeliveredModel(IOrderApplication orderApplication)
        {
            _orderApplication = orderApplication;
        }

        public void OnGet()
        {
            SearchModel.Status = 3;
            Orders = _orderApplication.Search(SearchModel);
        }
    }
}