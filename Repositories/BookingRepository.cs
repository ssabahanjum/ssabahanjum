using Dapper;
using Hotel.Interfaces;
using Hotel.Models;

namespace Hotel.Repositories
{
    public class BookingRepository : IBooking
    {
        private readonly DapperContext _context;

        public BookingRepository(DapperContext context)
        {
            _context = context;
        }
             
        public async Task<BookingDetail> GetBookingByBookingReferenceNumber(int bookingReferenceNumber)
        {
        
            using var connection = _context.CreateConnection(); 
            var booking = await connection.QuerySingleOrDefaultAsync<BookingDetail>("GetBookingByBookingReferenceNumber", new { BookingReferenceNumber = bookingReferenceNumber },
                 commandType: System.Data.CommandType.StoredProcedure);
            return booking;
        }

        public async Task DeleteBookings()
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DeleteBookings", commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task InsertBooking(BookingWithoutId bookingWithoutId)
        {
            using var connection = _context.CreateConnection();
           
            await connection.ExecuteAsync("InsertBooking", new { DateFrom = bookingWithoutId.DateFrom,
                DateTo = bookingWithoutId.DateTo,
                RoomId = bookingWithoutId.RoomId,
                CheckIn = bookingWithoutId.CheckIn,
                CheckOut = bookingWithoutId.CheckOut,
                NumberOfPeople = bookingWithoutId.NumberOfPeople,
                Amount = bookingWithoutId.Amount
            },
            commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task UpdateBooking(int bookingReferenceNumber, DateTime checkIn, DateTime? checkOut)
        {
            using var connection = _context.CreateConnection();

            await connection.ExecuteAsync("UpdateBooking", new
            {
                BookingReferenceNumber = bookingReferenceNumber,
                CheckIn = checkIn,
                CheckOut = checkOut
            },

           commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}




