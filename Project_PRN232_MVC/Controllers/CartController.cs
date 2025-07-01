using BusinessObjects.Models;
using BusinessObjects.ModelsDTO.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace Project_PRN232_MVC.Controllers;

[Route("api/cart")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly Prn222BeverageWebsiteProjectContext _context;

    public CartController()
    {
        _context = new Prn222BeverageWebsiteProjectContext();
    }

    // [GET] api/cart/{userId}
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCart(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.ProductVariant)
                    .ThenInclude(pv => pv.ProductSize)
            .Include(c => c.CartItems)
                .ThenInclude(ci => ci.ProductVariant.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return Ok(new { CartItems = new List<object>() });

        return Ok(cart);
    }

    // [POST] api/cart/{userId}/add
    [HttpPost("{userId}/add")]
    public async Task<IActionResult> AddToCart(int userId, [FromBody] CartItemDto dto)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var item = cart.CartItems.FirstOrDefault(i => i.ProductVariantId == dto.ProductVariantId);
        if (item != null)
        {
            item.Quantity += dto.Quantity;
        }
        else
        {
            cart.CartItems.Add(new CartItem
            {
                ProductVariantId = dto.ProductVariantId,
                Quantity = dto.Quantity
            });
        }

        await _context.SaveChangesAsync();
        return Ok(cart);
    }

    // [PUT] api/cart/{userId}/update
    [HttpPut("{userId}/update")]
    public async Task<IActionResult> UpdateItem(int userId, [FromBody] CartItemDto dto)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return NotFound("Cart not found");

        var item = cart.CartItems.FirstOrDefault(i => i.ProductVariantId == dto.ProductVariantId);
        if (item == null)
            return NotFound("Item not found");

        item.Quantity = dto.Quantity;
        await _context.SaveChangesAsync();

        return Ok(cart);
    }

    // [DELETE] api/cart/{userId}/remove/{variantId}
    [HttpDelete("{userId}/remove/{variantId}")]
    public async Task<IActionResult> RemoveItem(int userId, int variantId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return NotFound("Cart not found");

        var item = cart.CartItems.FirstOrDefault(i => i.ProductVariantId == variantId);
        if (item == null)
            return NotFound("Item not found");

        cart.CartItems.Remove(item);
        await _context.SaveChangesAsync();

        return Ok(cart);
    }

    // [DELETE] api/cart/{userId}/clear
    [HttpDelete("{userId}/clear")]
    public async Task<IActionResult> ClearCart(int userId)
    {
        var cart = await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
            return NotFound("Cart not found");

        cart.CartItems.Clear();
        await _context.SaveChangesAsync();

        return Ok("Cart cleared");
    }
}
