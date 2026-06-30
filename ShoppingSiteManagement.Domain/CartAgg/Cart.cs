using _0_Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Domain.CartAgg
{
    public class Cart : EntityBase<long>
    {
        public string AccountEmail { get; private set; }
        public bool IsFinished { get; private set; }
        public double TotalAmount { get; private set; }
        public double PayableAmount { get; private set; }

        public List<CartItem> Items { get; private set; }

        protected Cart() { }

        public Cart(string accountEmail)
        {
            AccountEmail = accountEmail;
            IsFinished = false;
            Items = new List<CartItem>();
        }
        public void Finish()
        {
            IsFinished = true;
        }

        public void Reopen()
        {
            IsFinished = false;
        }

        public void CalculateTotalAmount()
        {
            TotalAmount = 0;
            foreach (var item in Items)
            {
                TotalAmount += item.TotalPrice;
            }
            PayableAmount = TotalAmount;
        }
    }
}
