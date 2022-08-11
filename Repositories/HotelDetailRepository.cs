using Dapper;
using Hotel.Interfaces;
using Hotel.Models;

namespace Hotel.Repositories
{
    public class HotelDetailRepository : IHotel
    {
        private readonly DapperContext _context;

        public HotelDetailRepository(DapperContext context)
        {
            _context = context;
        }
             
        public async Task<HotelDetails> GetByHotelName(string hotelName)
        {
            System.Data.IDbConnection dbConnection = _context.CreateConnection();
            using var connection = dbConnection;
            var certificate = await connection.QuerySingleOrDefaultAsync<HotelDetails>("GetHotelDetailByName", new { hotelName=hotelName },
                 commandType: System.Data.CommandType.StoredProcedure);
            return certificate;
        }

     
    }
}




