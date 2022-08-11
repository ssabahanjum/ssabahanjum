namespace Hotel.Models
{
    public class BookingWithoutId
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int RoomId { get; set; }
        public DateTime? CheckIn { get; set; } = null;
        public DateTime? CheckOut { get; set; } = null;
        public int NumberOfPeople { get; set; }
        public decimal Amount { get; set; }

    }
  

    public class BookingDetail : BookingWithoutId
    {
        public string RoomType { get; set; }
        public string RoomName { get; set; }
        public int BookingReferenceNumber { get; set; }
    }
}
