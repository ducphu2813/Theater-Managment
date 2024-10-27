﻿using AdminService.Entity.Model;
using AdminService.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AdminService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieSaleController : ControllerBase
{
    
    private readonly IMovieSaleService _movieSaleService;
    
    public MovieSaleController(IMovieSaleService movieSaleService)
    {
        _movieSaleService = movieSaleService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllMovieSale()
    {
        var movieSales = await _movieSaleService.GetAllMovieSaleAsync();
        
        return Ok(movieSales);
    }
    
    // [HttpGet("{id}")]
    // public async Task<IActionResult> GetMovieSaleById(string id)
    // {
    //     var movieSale = await _movieSaleService.GetMovieSaleByIdAsync(id);
    //     
    //     if (movieSale == null)
    //     {
    //         return NotFound();
    //     }
    //     
    //     return Ok(movieSale);
    // }
    
    // [HttpPost]
    // public async Task<IActionResult> AddMovieSale([FromBody] MovieSale movieSale)
    // {
    //     var newMovieSale = await _movieSaleService.AddMovieSaleAsync(movieSale);
    //     
    //     return CreatedAtAction(nameof(GetMovieSaleById), new { id = newMovieSale.Id }, newMovieSale);
    // }
    //
    // [HttpPut("{id}")]
    // public async Task<IActionResult> UpdateMovieSale(string id, [FromBody] MovieSale movieSale)
    // {
    //     var updatedMovieSale = await _movieSaleService.UpdateMovieSaleAsync(id, movieSale);
    //     
    //     return Ok(updatedMovieSale);
    // }
    
    // [HttpDelete("{id}")]
    // public async Task<IActionResult> RemoveMovieSale(string id)
    // {
    //     var result = await _movieSaleService.RemoveMovieSaleAsync(id);
    //     
    //     if (!result)
    //     {
    //         return NotFound();
    //     }
    //     
    //     return Ok();
    // }
    
}