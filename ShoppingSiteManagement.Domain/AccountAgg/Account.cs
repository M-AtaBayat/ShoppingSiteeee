using _0_Framework.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Domain.AccountAgg
{
    public class Account : EntityBase<long>
    {
        public string Email { get; private set; }
        public string VerificationCode { get; private set; }
        public int RoleId { get; private set; }
        public bool IsActive { get; private set; }

        public Account(string email, int roleId = 2)
        {
            Email = email;
            RoleId = roleId;
            IsActive = true;
        }

        public void GenerateVerificationCode()
        {
            var random = new Random();
            VerificationCode = random.Next(1000, 9999).ToString();
        }
    }
}
