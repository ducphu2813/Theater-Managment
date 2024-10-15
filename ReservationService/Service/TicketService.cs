
using System.Text.Json;
using MongoDB.Bson.IO;
using ReservationService.DTO;
using ReservationService.Entity.Model;
using ReservationService.Exceptions;
using ReservationService.Repository.Interface;
using ReservationService.Service.Interface;
using ReservationService.External.Model;

namespace ReservationService.Service;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ISeatRepository _seatRepository;
    private readonly HttpClient _httpClient;
    
    public TicketService(
        ITicketRepository ticketRepository,
        IHttpClientFactory httpClientFactory,
        ISeatRepository seatRepository)
    {
        _ticketRepository = ticketRepository;
        _httpClient = httpClientFactory.CreateClient("movie-service");
        _seatRepository = seatRepository;
    }

    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _ticketRepository.GetAll();
    }
    
    //lấy danh sách vé theo id lịch chiếu
    public async Task<List<Ticket>> GetByScheduleIdAsync(string scheduleId)
    {
        return await _ticketRepository.GetByScheduleIdAsync(scheduleId);
    }

    //lấy theo id
    public async Task<Dictionary<String, Object>> GetByIdAsync(string id)
    {
        var ticket = await _ticketRepository.GetById(id);
        
        if (ticket == null)
        {
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }
        
        Console.WriteLine(ticket.MovieScheduleId);
        
        //gọi api của movie service để lấy thông tin phim
        var response = await _httpClient.GetAsync($"/api/MovieSchedule/{ticket.MovieScheduleId}");
        
        //cái này để chuyển từ snake_case sang camelCase
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        
        var movieScheduleContent = await response.Content.ReadAsStringAsync();
        var movieSchedule = JsonSerializer.Deserialize<MovieSchedule>(movieScheduleContent, options);
        
        // Console.WriteLine(response);
        
        //gộp ticket và response thành 1 map để trả về
        Dictionary<String, Object> map = new Dictionary<string, object>();
        map.Add("ticket", ticket);
        map.Add("movieSchedule", movieSchedule);
        
        return map;
    }

    public async Task<Ticket> AddAsync(Ticket ticket)
    {
        return await _ticketRepository.Add(ticket);
    }

    public async Task<Ticket> UpdateAsync(string id, Ticket ticket)
    {
        Ticket ticketToUpdate = await _ticketRepository.GetById(id);
        
        if (ticketToUpdate == null)
        {
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }
        
        return await _ticketRepository.Update(id, ticket);
        
    }

    public async Task<bool> RemoveAsync(string id)
    {
        Ticket ticketToDelete = await _ticketRepository.GetById(id);
        
        if (ticketToDelete == null)
        {
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }
        
        return await _ticketRepository.Remove(id);
    }
    
    //hàm lấy tất cả SeatDetail theo id lịch chiếu
    public async Task<Dictionary<String, Object>> GetAllBookedSeatByScheduleIdAsync(string scheduleId, string roomNumber){
        
        Dictionary<String, Object> map = new Dictionary<string, object>();
        
        Console.WriteLine("id lich chieu truyen vao: "+scheduleId);
        
        //lấy tất cả vé theo id lịch chiếu
        var tickets = await _ticketRepository.GetByScheduleIdAsync(scheduleId);
        
        //lấy tất cả ghế đã đặt
        var seatDetails = tickets.SelectMany(t => t.SeatDetail).ToList();
        
        //lấy tất cả danh sách ghế trong room theo room number
        var seats = await _seatRepository.GetByRoomNumberAsync(roomNumber);
        
        map.Add("allSeats", seats);
        map.Add("bookedSeat", seatDetails);
        
        return map;
        
    }
}