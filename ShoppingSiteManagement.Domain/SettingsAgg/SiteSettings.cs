using _0_Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Domain.SettingsAgg
{
    public class SiteSettings : EntityBase<int>
    {
        public double ShippingCost { get; private set; }
        public string AdminEmail { get; private set; }

        protected SiteSettings() { }

        public SiteSettings(double shippingCost, string adminEmail)
        {
            ShippingCost = shippingCost;
            AdminEmail = adminEmail;
        }

        public void ChangeShippingCost(double newCost)
        {
            ShippingCost = newCost;
        }
    }
}
