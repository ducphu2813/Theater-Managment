﻿using AdminService.Entity.Model;
using AdminService.Exception;
using AdminService.Repository.Interface;
using AdminService.Service.Interface;

namespace AdminService.Service;

public class MovieSaleService : IMovieSaleService
{
    private readonly IMovieSaleRepository _movieSaleRepository;
    
    public MovieSaleService(IMovieSaleRepository movieSaleRepository)
    {
        _movieSaleRepository = movieSaleRepository;
    }
    
    public async Task<IEnumerable<MovieSale>> GetAllMovieSaleAsync()
    {
        return await _movieSaleRepository.GetAll();
    }

    public async Task<MovieSale> GetMovieSaleByIdAsync(string id)
    {
        var movieSale = await _movieSaleRepository.GetById(id);
        
        if (movieSale == null)
        {
            throw new NotFoundException($"Movie Sale with id {id} was not found.");
        }
        
        return movieSale;
    }

    public async Task<MovieSale> AddMovieSaleAsync(MovieSale movieSale)
    {
        return await _movieSaleRepository.Add(movieSale);
    }

    public async Task<MovieSale> UpdateMovieSaleAsync(string id, MovieSale movieSale)
    {
        var existingMovieSale = await _movieSaleRepository.GetById(id);
        
        if (existingMovieSale == null)
        {
            throw new NotFoundException($"Movie Sale with id {id} was not found.");
        }
        
        return await _movieSaleRepository.Update(id, movieSale);
    }

    public async Task<bool> RemoveMovieSaleAsync(string id)
    {
        var existingMovieSale = await _movieSaleRepository.GetById(id);
        
        if (existingMovieSale == null)
        {
            throw new NotFoundException($"Movie Sale with id {id} was not found.");
        }
        
        return await _movieSaleRepository.Remove(id);
    }
}