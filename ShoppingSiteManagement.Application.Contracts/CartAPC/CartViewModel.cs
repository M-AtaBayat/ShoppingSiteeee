using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.Contracts.CartAPC
{
    public class CartViewModel
    {
        public long Id { get; set; }
        public double TotalAmount { get; set; }
        public double PayableAmount { get; set; }
        public int TotalItems { get; set; }
        public List<CartItemViewModel> Items { get; set; }
    }
}
