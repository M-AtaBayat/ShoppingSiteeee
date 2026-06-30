namespace ShoppingSiteManagement.Application.Contracts.OrderAPC
{
    public class SearchOrderModel
    {
        public string TrackingCode { get; set; }
        public string PostTrackingCode { get; set; }
        public int Status { get; set; }
        public string ReceiverName { get; set; }
    }
}
