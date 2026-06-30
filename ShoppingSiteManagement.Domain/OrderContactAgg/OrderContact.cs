using _0_Framework.Domain;

namespace ShoppingSiteManagement.Domain.OrderContactAgg
{
    public class OrderContact : EntityBase<long>
    {
        public string PhoneNumber { get; private set; }
        public string TrackingCode { get; private set; }
        public string Message { get; private set; }
        public bool IsRead { get; private set; }

        protected OrderContact() { }

        public OrderContact(string phoneNumber, string trackingCode, string message)
        {
            PhoneNumber = phoneNumber;
            TrackingCode = trackingCode;
            Message = message;
            IsRead = false;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}