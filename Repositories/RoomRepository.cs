using Dapper;
using Hotel.Interfaces;
using Hotel.Models;

namespace Hotel.Repositories
{
    public class RoomRepository : IRoom
    {
        private readonly DapperContext _context;

        public RoomRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<int> GetRoomCapacity(int roomId)
        {
            using var connection = _context.CreateConnection();
            var roomCapacity = await connection.QuerySingleOrDefaultAsync<int>("GetRoomCapacity", new { Id = roomId },
                 commandType: System.Data.CommandType.StoredProcedure);
            return roomCapacity;
        }

        public async Task<List<RoomAvailable>> GetRoomsAvailable(DateTime dateFrom, DateTime dateTo, int numberOfPeople)
        {
            using var connection = _context.CreateConnection();
            var roomsAvailable = await connection.QueryAsync<RoomAvailable>("GetRoomsAvailable", new { DateFrom = dateFrom, DateTo = dateTo, NumberOfPeople = numberOfPeople },
                 commandType: System.Data.CommandType.StoredProcedure);
            return roomsAvailable.ToList();
        }
       

    }
}




