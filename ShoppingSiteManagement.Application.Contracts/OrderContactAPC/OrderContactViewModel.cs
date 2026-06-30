namespace ShoppingSiteManagement.Application.Contracts.OrderContactAPC
{
    public class OrderContactViewModel
    {
        public long Id { get; set; }
        public string PhoneNumber { get; set; }
        public string TrackingCode { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
        public string CreationDate { get; set; }
    }
}