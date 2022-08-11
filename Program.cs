using Microsoft.AspNetCore.Mvc;
using Hotel.Interfaces;
using Hotel;
using Hotel.Repositories;
using Hotel.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<DapperContext>();
builder.Services.AddScoped<IRoom, RoomRepository>();
builder.Services.AddScoped<IHotel, HotelDetailRepository>();
builder.Services.AddScoped<IBooking, BookingRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
bool showSwaggerUi;
if (bool.TryParse(app.Configuration.GetSection("Swagger")["ShowUi"], out showSwaggerUi)
    && showSwaggerUi)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/hotelDetails/{hotelName}", async ([FromServices] IHotel repo, string hotelName) =>
{
    var hotelDetails = await repo.GetByHotelName(hotelName);
    return hotelDetails is not null ? Results.Ok(hotelDetails) : Results.Ok("Hotel Details not found");
});

app.MapGet("/bookingDetails/{bookingReferenceNumber}", async ([FromServices] IBooking repo, int bookingId) =>
{
    var bookingDetail = await repo.GetBookingByBookingReferenceNumber(bookingId);
    return bookingDetail is not null ? Results.Ok(bookingDetail) : Results.Ok("No booking found");
});

app.MapDelete("/bookings", async ([FromServices] IBooking repo) =>
{
    await repo.DeleteBookings(); 
    return Results.Ok("Bookings deleted");

});

app.MapGet("/roomsAvailable/{dateFrom}/{dateTo}/{numberOfPeople}", async ([FromServices] IRoom repo, DateTime dateFrom, DateTime dateTo, int numberOfPeople) =>
{
    var roomsAvailable = await repo.GetRoomsAvailable(dateFrom, dateTo, numberOfPeople);
    return roomsAvailable is not null ? Results.Ok(roomsAvailable) : Results.Ok("Hotel Details not found");
}); 

app.MapPost("/booking", async ([FromServices] IBooking repo, IRoom roomrepo, BookingWithoutId bookingWithoutId) =>
{
    var roomsAvailable = await roomrepo.GetRoomsAvailable(bookingWithoutId.DateFrom, bookingWithoutId.DateTo, bookingWithoutId.NumberOfPeople);
    var roomCapacity = await roomrepo.GetRoomCapacity(bookingWithoutId.RoomId);
    if (bookingWithoutId.NumberOfPeople <= 0 )
    {
        return Results.Ok("Your Booking should have number of people");
    }

    if (roomCapacity < bookingWithoutId.NumberOfPeople)
    {
        return Results.Ok("Your Booking is unsuccesful because number of people are more than room's capacity or room id doesn't exist");
    }

    bool roomAvailables = roomsAvailable.Any(p => p.RoomId == bookingWithoutId.RoomId);
    if (roomAvailables == false)
    {
        return Results.Ok("Room has already booked for the given date");
    }

    int result = DateTime.Compare(bookingWithoutId.DateTo, bookingWithoutId.DateFrom);
    if (result < 0)
    {
        return Results.Ok("DateFrom should be earlier than DateTo");
    }

    await repo.InsertBooking(bookingWithoutId);
    return Results.Ok(bookingWithoutId +"Room booked");
});

app.MapPost("/seeding", async ([FromServices] IBooking repo, IRoom roomrepo) =>
{
    BookingWithoutId bookingWithoutId = new BookingWithoutId();
    bookingWithoutId.DateFrom = DateTime.Today;
    bookingWithoutId.DateTo = DateTime.Today.AddDays(1);
    bookingWithoutId.NumberOfPeople = 2;
 
    var roomsAvailable = await roomrepo.GetRoomsAvailable(bookingWithoutId.DateFrom, bookingWithoutId.DateTo, bookingWithoutId.NumberOfPeople);

    for(int i = 0; i < roomsAvailable.Count; i++)
    {
        bookingWithoutId.RoomId = roomsAvailable[i].RoomId;
        var roomCapacity = await roomrepo.GetRoomCapacity(bookingWithoutId.RoomId);
        int result = DateTime.Compare(bookingWithoutId.DateTo, bookingWithoutId.DateFrom);

        if (bookingWithoutId.NumberOfPeople > 0 && roomCapacity >= bookingWithoutId.NumberOfPeople)
        {
            await repo.InsertBooking(bookingWithoutId);
        }
    }
    return Results.Ok("Records added");
});

app.MapPut("/booking/{bookingReferenceNumber}/{checkIn}/{checkOut}", async ([FromServices] IBooking repo, int  bookingReferenceNumber, DateTime checkIn, DateTime checkOut) =>
{
    var booking = await repo.GetBookingByBookingReferenceNumber(bookingReferenceNumber);
    if (booking is null)
    {
        return Results.Ok("Booking Details not found");
    }

    int result = DateTime.Compare(checkOut, checkIn);
    if (result < 0)
    {
        return Results.Ok("Check In should be earlier than Check Out");
    }

    await repo.UpdateBooking(bookingReferenceNumber, checkIn, checkOut);
    return Results.Ok("Booking updated with checkIn checkOut");
});

app.Run();