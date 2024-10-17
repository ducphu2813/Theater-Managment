
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
    private readonly IFoodRepository _foodRepository;
    private readonly IDiscountRepository _discountRepository;
    private readonly HttpClient _httpClient;
    
    public TicketService(
        ITicketRepository ticketRepository,
        IHttpClientFactory httpClientFactory,
        ISeatRepository seatRepository,
        IFoodRepository foodRepository,
        IDiscountRepository discountRepository)
    {
        _ticketRepository = ticketRepository;
        _httpClient = httpClientFactory.CreateClient("movie-service");
        _seatRepository = seatRepository;
        _foodRepository = foodRepository;
        _discountRepository = discountRepository;
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

    //lấy theo id ticket
    public async Task<Dictionary<String, Object>> GetByIdAsync(string id)
    {
        var ticket = await _ticketRepository.GetById(id);
        
        if (ticket == null)
        {
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }
        
        Console.WriteLine(ticket.MovieScheduleId);
        
        //gọi api của movie service để lấy thông tin phim
        var response = await _httpClient.GetAsync($"/api/MovieSchedule/schedule/{ticket.MovieScheduleId}");
        
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

    
    public async Task<Ticket> AddAsync(SaveTicketDTO ticket)
    {
        //Note
        // 1. Khi mua cần kiểm tra schedule id và seat id đã được đặt chưa
        
        //lấy schedule id và seat id để kiểm tra
        var ticketList = await _ticketRepository.GetByScheduleIdAndSeatIdAsync(ticket.MovieScheduleId, ticket.SeatId);
        Console.WriteLine("tìm thấy "+ticketList.Count+" vé");
        //lấy thông tin chi tiết các ghế đã được đặt
        List<Seat> seatDetails = new List<Seat>();
        foreach (var t in ticketList)
        {
            seatDetails.AddRange(t.SeatDetail);
        }
        if(ticketList.Count != 0)
        {
            throw new InvalidOperationException("Seat has been booked: " + string.Join(", ", seatDetails.Select(s => s.ToString())));
        }
        
        //tạo ticket mới
        Ticket newTicket = new Ticket
        {
            MovieScheduleId = ticket.MovieScheduleId,
            SeatId = ticket.SeatId,
            FoodId = ticket.FoodId,
            Status = "pending",
            TotalPrice = ticket.TotalPrice,
            TotalTicket = ticket.TotalTicket,
            UserId = ticket.UserId,
            CreatedAt = DateTime.Now
        };
        Console.WriteLine("vé mới tạo là : "+newTicket);
        
        //cập nhật lại SeatDetail, FoodDetail và DiscountDetail
        newTicket = await UpdateTicketDetail(newTicket);
        
        // return newTicket;
        return await _ticketRepository.Add(newTicket);
    }

    public async Task<Ticket> UpdateAsync(string id, UpdateTicketDTO updateTicketDto)
    {
        Ticket ticketToUpdate = await _ticketRepository.GetById(id);
        
        if (ticketToUpdate == null)
        {
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }
        
        //lấy tất cả property của update ticket DTO
        var updateProperties = typeof(UpdateTicketDTO).GetProperties();
        //lấy tất cả property của ticket
        var ticketProperties = typeof(Ticket).GetProperties();
        
        //duyệt qua từng property của update ticket DTO
        foreach (var property in updateProperties)
        {
            var newValue = property.GetValue(updateTicketDto);
            
            if(newValue != null)
            {
                var ticketProperty = ticketProperties.FirstOrDefault(p => p.Name == property.Name);
                
                if (ticketProperty != null && ticketProperty.CanWrite)
                {
                    ticketProperty.SetValue(ticketToUpdate, newValue);
                }
            }
            
        }
        
        //trước khi lưu ticket thì cần cập nhật lại SeatDetail, FoodDetail và DiscountDetail
        ticketToUpdate = await UpdateTicketDetail(ticketToUpdate);
        
        return await _ticketRepository.Update(id, ticketToUpdate);
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
    
    //xóa tất cả vé
    public async Task<bool> RemoveAllAsync()
    {
        return await _ticketRepository.RemoveAllAsync();
    }
    
    //hàm lấy tất cả SeatDetail theo id lịch chiếu
    //cái này dành cho bên movie service khi user muốn đặt mua vé
    public async Task<Dictionary<String, Object>> GetAllBookedSeatByScheduleIdAsync(string scheduleId, string roomNumber){
        
        //Note lại cái cần làm
        //1. Chỉ lấy những vé có status là confirmed(nghĩa là đã thanh toán)
        
        Dictionary<String, Object> map = new Dictionary<string, object>();
        
        Console.WriteLine("id lich chieu truyen vao: "+scheduleId);
        
        //lấy tất cả vé theo id lịch chiếu
        //chỉ lấy những vé có status là confirmed
        var tickets = await _ticketRepository.GetByScheduleIdAsync(scheduleId);
        
        //lấy tất cả ghế đã đặt
        var seatDetails = tickets.SelectMany(t => t.SeatDetail).ToList();
        
        //lấy tất cả danh sách ghế trong room theo room number
        var seats = await _seatRepository.GetByRoomNumberAsync(roomNumber);
        
        map.Add("allSeats", seats);
        map.Add("bookedSeat", seatDetails);
        
        return map;
        
    }
    
    //hàm hỗ trợ
    //hàm cập nhật các Seat, Food và Discount của ticket
    private async Task<Ticket> UpdateTicketDetail(Ticket ticket)
    {
        //kiểm tra SeatId của ticket có null không
        if (ticket.SeatId != null)
        {
            Console.WriteLine("seat id là : "+ticket.SeatId);
            //lấy tất cả seat id trong ticket
            var seatIds = ticket.SeatId;
        
            //lấy tất cả seat theo id
            var seats = await _seatRepository.GetByIdsAsync(seatIds);
            Console.WriteLine("tìm thấy "+seats.Count+" seat");
            
            //nếu không có seat nào thì được tìm thấy theo id thì không làm gì cả
            if (seats.Count != 0)
            {
                //cập nhật lại seat detail
                ticket.SeatDetail = seats;
                Console.WriteLine("seat detail của ticket là : "+ticket.SeatDetail);
            }
        }
        
        //kiểm tra xem FoodId của ticket có null không
        if (ticket.FoodId != null)
        {
            Console.WriteLine("food id là : "+ticket.FoodId);
            //lấy tất cả food id trong ticket
            var foodIds = ticket.FoodId;
            
            //lấy tất cả food theo id
            var foods = await _foodRepository.GetByFoodIdAsync(foodIds);
            Console.WriteLine("tìm thấy "+foods.Count+" food");
            
            //nếu không có food nào thì được tìm thấy theo id thì không làm gì cả
            if (foods.Count != 0)
            {
                //cập nhật lại food
                ticket.FoodDetail = foods;
                Console.WriteLine("food detail của ticket là : "+ticket.FoodDetail);
            }
        }
        
        //kiểm tra xem DiscountId của ticket có null không
        if (ticket.FoodId != null && ticket.SeatId != null)
        {
            //lấy danh sách các FoodType và SeatType trong ticket
            var foodTypes = ticket.FoodDetail.Select(f => f.FoodType).ToList();
            var seatTypes = ticket.SeatDetail.Select(s => s.SeatType).ToList();
            //lấy discount theo id
            var discount = await _discountRepository.GetByFoodTypeAndSeatTypeAsync(foodTypes, seatTypes);
            
            //nếu không có discount nào thì được tìm thấy theo id thì không làm gì cả
            if (discount != null)
            {
                //lấy discount lớn nhất
                var maxDiscount = discount.OrderByDescending(d => d.PercentOff).FirstOrDefault();
                
                //cập nhật lại discount
                ticket.DiscountDetail = maxDiscount;
                ticket.DiscountId = maxDiscount.Id;
            }
        }
        
        return ticket;
    }
    
}