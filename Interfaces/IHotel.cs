using Hotel.Models;
namespace Hotel.Interfaces;

public interface IHotel
{

    Task<HotelDetails> GetByHotelName(string hotelName);
  
}
