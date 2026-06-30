using _0_Framework.Application;
using ShoppingSiteManagement.Application.Contracts.OrderContactAPC;
using ShoppingSiteManagement.Domain.OrderContactAgg;
using System.Collections.Generic;

namespace ShoppingSiteManagement.Application.OrderContactAPP
{
    public class OrderContactApplication : IOrderContactApplication
    {
        private readonly IOrderContactRepository _orderContactRepository;

        public OrderContactApplication(IOrderContactRepository orderContactRepository)
        {
            _orderContactRepository = orderContactRepository;
        }

        public OperationResult Register(RegisterOrderContact command)
        {
            var operation = new OperationResult();
            var contact = new OrderContact(command.PhoneNumber, command.TrackingCode, command.Message);

            _orderContactRepository.Add(contact);
            _orderContactRepository.Save();

            return operation.Success("درخواست شما ثبت شد. به زودی با شما تماس می‌گیریم.");
        }

        public OperationResult MarkAsRead(long id)
        {
            var operation = new OperationResult();
            var contact = _orderContactRepository.Get(id);

            if (contact == null)
                return operation.Failed("رکورد مورد نظر یافت نشد.");

            contact.MarkAsRead();
            _orderContactRepository.Save();

            return operation.Success();
        }

        public List<OrderContactViewModel> GetList()
        {
            return _orderContactRepository.GetList();
        }
    }
}