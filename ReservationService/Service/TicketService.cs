
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

    //lấy theo id ticket, cái này lấy chi tiết cả movie schedule và movie(có cả id)
    public async Task<Dictionary<String, Object>> GetByIdAsync(string id)
    {
        var ticket = await _ticketRepository.GetById(id);
        
        if (ticket == null)
        {
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }
        
        Console.WriteLine(ticket.MovieScheduleId);
        
        //gọi api của movie service để lấy thông tin phim và lịch chiếu
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
        //Lấy ghế theo danh sách id ghế
        List<Seat> seats = await _seatRepository.GetByIdsAsync(ticket.SeatId);
        if(seats.Count == 0)
        {
            throw new NotFoundException("Seat not found");
        }
        
        //Note
        // [DONE] 1. Khi mua cần kiểm tra schedule id và seat id đã được đặt chưa
        // Dùng schedule id lấy schedule và làm những việc sau: 
        // 2. Kiểm tra ghế truyền vào có đúng với số phòng của schedule ko
        // 3. Dùng schedule id để lấy giá ghế đơn và giá ghế đôi
        
        //gọi api của movie service để lấy thông tin phim và lịch chiếu
        var response = await _httpClient.GetAsync($"/api/MovieSchedule/schedule/{ticket.MovieScheduleId}");
        //kiểm tra response có lỗi không
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException("Movie schedule not found");
        }
        //cái này để chuyển từ snake_case sang camelCase
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var movieScheduleContent = await response.Content.ReadAsStringAsync();
        var movieSchedule = JsonSerializer.Deserialize<MovieSchedule>(movieScheduleContent, options);
        
        //kiểm tra room number trong movie schedule có trùng với room number trong seats không
        if(movieSchedule.RoomNumber != seats.FirstOrDefault().RoomNumber)
        {
            throw new InvalidOperationException("Room number and seat is not match");
        }
        
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
        
        //lấy single seat price và double seat price lưu thành 1 dictionary
        Dictionary<String, int> seatPrice = new Dictionary<string, int>();
        seatPrice.Add("singleSeatPrice", movieSchedule.SingleSeatPrice ?? 0);
        seatPrice.Add("coupleSeatPrice", movieSchedule.CoupleSeatPrice ?? 0);
        
        //tạo ticket mới
        Ticket newTicket = new Ticket
        {
            MovieScheduleId = ticket.MovieScheduleId,
            SeatId = ticket.SeatId,
            FoodId = ticket.FoodId,
            Status = "pending",
            BaseAmount = 0,
            TotalAmount = 0,
            UserId = ticket.UserId,
            CreatedAt = DateTime.Now,
            ExpiryTime = DateTime.Now.AddMinutes(5)
        };
        Console.WriteLine("vé mới tạo là : "+newTicket);
        
        //cập nhật lại SeatDetail, FoodDetail và DiscountDetail
        newTicket = await UpdateTicketDetail(newTicket, seats, seatPrice);
        
        // return newTicket;
        return await _ticketRepository.Add(newTicket);
    }

    public async Task<Ticket> UpdateAsync(string id, UpdateTicketDTO updateTicketDto)
    {
        Ticket ticketToUpdate = await _ticketRepository.GetById(id);
        
        //kiểm tra ticket tồn tại
        if (ticketToUpdate == null)
        {
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }
        
        //kiểm tra ticket có status là processed không
        if(ticketToUpdate.Status == "processed")
        {
            throw new InvalidOperationException("processed Ticket cannot be updated");
        }
        
        //kiểm tra UserId trong ticketToUpdate có khác với UserId trong updateTicketDto không
        if(ticketToUpdate.UserId != updateTicketDto.UserId)
        {
            throw new InvalidOperationException("This is not your ticket");
        }
        
        //Lấy ghế theo danh sách id ghế
        List<Seat> seats = await _seatRepository.GetByIdsAsync(updateTicketDto.SeatId);
        if(seats.Count == 0)
        {
            throw new NotFoundException("Seat not found");
        }
        
        //gọi api của movie service để lấy thông tin phim và lịch chiếu
        var response = await _httpClient.GetAsync($"/api/MovieSchedule/schedule/{updateTicketDto.MovieScheduleId}");
        //cái này để chuyển từ snake_case sang camelCase
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        var movieScheduleContent = await response.Content.ReadAsStringAsync();
        var movieSchedule = JsonSerializer.Deserialize<MovieSchedule>(movieScheduleContent, options);
        
        //kiểm tra room number trong movie schedule có trùng với room number trong seats không
        if(movieSchedule.RoomNumber != seats.FirstOrDefault().RoomNumber)
        {
            throw new InvalidOperationException("Room number and seat is not match");
        }
        
        //lấy schedule id và seat id để kiểm tra, cái này sẽ chỉ lấy những vé đã được thanh toán (status là processed)
        var ticketList = await _ticketRepository.GetByScheduleIdAndSeatIdAsync(updateTicketDto.MovieScheduleId, updateTicketDto.SeatId);
        Console.WriteLine("tìm thấy "+ticketList.Count+" vé");
        //lấy thông tin chi tiết các ghế đã được đặt
        List<Seat> seatDetails = new List<Seat>(); 
        foreach (var t in ticketList)
        {
            seatDetails.AddRange(t.SeatDetail);
        }
        if(ticketList.Count != 0)
        {
            throw new InvalidOperationException("Seat has been booked: " +seatDetails.Select(s => s.ToString()));
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
        
        //lấy single seat price và double seat price lưu thành 1 dictionary
        Dictionary<String, int> seatPrice = new Dictionary<string, int>();
        seatPrice.Add("singleSeatPrice", movieSchedule.SingleSeatPrice ?? 0);
        seatPrice.Add("coupleSeatPrice", movieSchedule.CoupleSeatPrice ?? 0);
        
        //trước khi lưu ticket thì cần cập nhật lại SeatDetail, FoodDetail và DiscountDetail
        ticketToUpdate = await UpdateTicketDetail(ticketToUpdate, seats, seatPrice);
        
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
        //1. Chỉ lấy những vé có status là processed(nghĩa là đã thanh toán)
        
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
    private async Task<Ticket> UpdateTicketDetail(Ticket ticket, List<Seat> seats, Dictionary<String, int> seatPrice)
    {
        //NOte: cần cập nhật cả giá vé, tính luôn giảm giá nếu có
        //kiểm tra SeatId của ticket có null không
        // if (ticket.SeatId != null)
        // {
        //     Console.WriteLine("seat id là : "+ticket.SeatId);
        //     //lấy tất cả seat id trong ticket
        //     var seatIds = ticket.SeatId;
        //
        //     //lấy tất cả seat theo id
        //     var seats = await _seatRepository.GetByIdsAsync(seatIds);
        //     Console.WriteLine("tìm thấy "+seats.Count+" seat");
        //     
        //     //nếu không có seat nào thì được tìm thấy theo id thì không làm gì cả
        //     if (seats.Count != 0)
        //     {
        //         //cập nhật lại seat detail
        //         ticket.SeatDetail = seats;
        //         Console.WriteLine("seat detail của ticket là : "+ticket.SeatDetail);
        //     }
        // }
        
        ticket.SeatDetail = seats;
        Console.WriteLine("seat detail của ticket là : "+ticket.SeatDetail);
        
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
                
                //tính base amount cho ticket dựa trên số food tìm thấy
                float foodAmount = 0;
                foreach (var food in ticket.FoodDetail)
                {
                    foodAmount += (float)food.Amount;
                }
                ticket.BaseAmount += foodAmount;
            }
        }
        
        //tính base amount cho ticket dựa trên số lượng ghế
        float seatAmount = 0;
        foreach (var seat in ticket.SeatDetail)
        {
            if (seat.SeatType == "single")
            {
                seatAmount += (float)seatPrice["singleSeatPrice"];
            }
            else
            {
                seatAmount += (float)seatPrice["coupleSeatPrice"];
            }
        }
        ticket.BaseAmount += seatAmount;
        
        //kiểm tra xem DiscountId của ticket có null không
        if (ticket.FoodId != null)
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
                
                Console.WriteLine("TÌm thấy discount là : "+maxDiscount.PercentOff);
                
                //tính total amount cho ticket dựa trên base amount và discount
                ticket.TotalAmount = ticket.BaseAmount - (ticket.BaseAmount * (float)(maxDiscount.PercentOff / 100.0f));
            }
            else
            {
                Console.WriteLine("Không tìm thấy discount phù hợp");
                ticket.TotalAmount = ticket.BaseAmount;
            }
        }
        else
        {
            Console.WriteLine("Không đặt đồ ăn nên không có discount");
            ticket.TotalAmount = ticket.BaseAmount;
        }
        
        return ticket;
    }
    
}