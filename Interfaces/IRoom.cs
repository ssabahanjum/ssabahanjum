using Hotel.Models;
namespace Hotel.Interfaces;

public interface IRoom
{
    Task<int> GetRoomCapacity(int roomId);
    Task<List<RoomAvailable>> GetRoomsAvailable(DateTime dateFrom, DateTime dateTo, int numberOfPeople);
   
}
