using AutoMapper;
using LibrarySystem.Data;
using LibrarySystem.Dtos;
using LibrarySystem.Models;
using LibrarySystem.Services.Implementations;
using LibrarySystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICategoryService _categoryService;
        private readonly IBookService _bookService;

        public CategoriesController(ApplicationDbContext context, ICategoryService categoryService, IBookService bookService)
        {
            _context = context;
            _categoryService = categoryService;
            _bookService = bookService;
        }

        // GET: api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories()
        {
            var categories = await _categoryService.GetAll();
            var categoryDtos = categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList();
            return Ok(categoryDtos);
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = new CategoryDto { Id = category.Id, Name = category.Name };
            return Ok(categoryDto);
        }

        // POST: api/categories
        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategoryDto categoryDto)
        {
            var category = new Category { Name = categoryDto.Name };
            await _categoryService.Add(category);
            categoryDto.Id = category.Id;
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, categoryDto);
        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> UpdateCategory(int id, CategoryDto categoryDto)
        {
            if (id != categoryDto.Id)
            {
                return BadRequest();
            }
            var category = await _categoryService.GetById(id);
            if (category == null)
            {
                return NotFound();
            }
            category.Name = categoryDto.Name;
            await _categoryService.Update(category);
            return NoContent();
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            
            var ret  =await _categoryService.Delete(id);
            if (ret == -1)
            {
                return NotFound();
            }
            return NoContent();
        }

        // GET: api/categories/{id}/books
        [HttpGet("{id}/books")]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooksByCategory(int id)
        {
            var category = await _categoryService.GetById(id);

            if (category == null)
            {
                return NotFound("Category not found");
            }

            var allBooks = await _bookService.GetAll();
            var books = allBooks.Where(b => b.CategoryId == id);
            var bookDtos = books.Select(b => new BookDto { Id = b.Id, Title = b.Title, Author = b.Author, Category = new CategoryDto { Id = category.Id, Name = category.Name } }).ToList();

            return Ok(bookDtos);
        }

        private bool CategoryExists(int id)
        {
            var category = _categoryService.GetById(id);
            if (category != null)
                return true;
            else 
                return false;
        }
    }
}
