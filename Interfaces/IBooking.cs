using Hotel.Models;
namespace Hotel.Interfaces;

public interface IBooking
{
    Task<BookingDetail> GetBookingByBookingReferenceNumber(int bookingId);
    Task DeleteBookings();
    Task InsertBooking(BookingWithoutId bookingWithoutId);
    Task UpdateBooking(int bookingReferenceNumber, DateTime checkIn, DateTime? checkOut);
}
