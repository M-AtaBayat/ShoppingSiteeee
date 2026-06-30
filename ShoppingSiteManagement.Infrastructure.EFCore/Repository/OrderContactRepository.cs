using _0_Framework.Application;
using _0_Framework.Infrastructure;
using ShoppingSiteManagement.Application.Contracts.OrderContactAPC;
using ShoppingSiteManagement.Domain.OrderContactAgg;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ShoppingSiteManagement.Infrastructure.EFCore.Repository
{
    public class OrderContactRepository : RepositoryBase<long, OrderContact>, IOrderContactRepository
    {
        private readonly ShoppingSiteContext _context;

        public OrderContactRepository(ShoppingSiteContext context) : base(context)
        {
            _context = context;
        }

        public List<OrderContactViewModel> GetList()
        {
            var persianCalendar = new PersianCalendar();
            return _context.OrderContacts
                .Select(x => new OrderContactViewModel
                {
                    Id = x.Id,
                    PhoneNumber = x.PhoneNumber,
                    TrackingCode = x.TrackingCode,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    CreationDate = persianCalendar.GetYear(x.CreationDate) + "/" +
                    persianCalendar.GetMonth(x.CreationDate).ToString("00") + "/" +
                    persianCalendar.GetDayOfMonth(x.CreationDate).ToString("00") + " " +
                    x.CreationDate.Hour.ToString("00") + ":" +
                    x.CreationDate.Minute.ToString("00") + ":" +
                    x.CreationDate.Second.ToString("00"),
                })
                .OrderBy(x => x.IsRead)
                .ThenByDescending(x => x.Id)
                .ToList();
        }
    }
}