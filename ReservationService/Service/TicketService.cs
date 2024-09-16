using ReservationService.Entity.Model;
using ReservationService.Exceptions;
using ReservationService.Repository.Interface;
using ReservationService.Service.Interface;

namespace ReservationService.Service;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    
    public TicketService(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }


    public async Task<IEnumerable<Ticket>> GetAllAsync()
    {
        return await _ticketRepository.GetAll();
    }

    public async Task<Ticket> GetByIdAsync(string id)
    {
        var ticket = await _ticketRepository.GetById(id);
        
        if (ticket == null)
        {
            throw new NotFoundException($"Ticket with id {id} was not found.");
        }
        
        return ticket;
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
}