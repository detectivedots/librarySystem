using LibrarySystem.Data;
using LibrarySystem.Dtos;
using LibrarySystem.Models;
using LibrarySystem.Services.Implementations;
using LibrarySystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowRequestsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBorrowRequestService _borrowRequestService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly List<string> _possibleStatus = new List<string>(["PENDING", "RETURNED", "BORROWED", "DECLINED"]);
        public BorrowRequestsController(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, IBorrowRequestService borrowRequestService)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _borrowRequestService = borrowRequestService;
        }

        // POST: api/borrowrequests
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BorrowRequest>> CreateBorrowRequest(UserBorrowRequestDto userBorrowRequestDto)
        {
            var user = await _userManager.FindByNameAsync(userBorrowRequestDto.UserName);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var request = new BorrowRequest
            {
                BookId = userBorrowRequestDto.BookId,
                UserId = user.Id,
                BorrowDate = userBorrowRequestDto.BorrowDate,
                ReturnDate = userBorrowRequestDto.ReturnDate,
                Status = "PENDING"
            };
            await _borrowRequestService.Add(request);
            return Ok(new BorrowRequestDto(request));
        }

        // GET: api/borrowrequests
        [HttpGet]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<IEnumerable<BorrowRequest>>> GetBorrowRequests()
        {
            var requests = await _borrowRequestService.GetAll();
            return Ok(requests);
        }

        // PUT: api/borrowrequests/{requestId}
        [HttpPut("{requestId}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> UpdateBorrowRequestStatus(int requestId, string status)
        {
            var request = await _borrowRequestService.GetById(requestId);
            if (request == null)
                return NotFound();
            status = status.ToUpper();
            if (!_possibleStatus.Contains(status))
            {
                return BadRequest("Invalid status");
            }
         
            request.Status = status;
            await _borrowRequestService.Update(request);
            return Ok();
        }

        [HttpGet("borrowed-books/{userName}")]
        public async Task<ActionResult<IEnumerable<UserBorrowRequestDto>>> GetBorrowedBooks(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return BadRequest("User not found");
            string id = user.Id;
            var borrowRequests = await _borrowRequestService.GetAll();
            var ret = borrowRequests.Where(b => b.UserId == id && b.Status == "BORROWED");
            List<UserBorrowRequestDto> lst = new List<UserBorrowRequestDto>();
            foreach (var b in ret)
            {
                lst.Add(new UserBorrowRequestDto(b));
            }
            return Ok(lst);
        }

        [HttpGet("pending-requests/{userName}")]
        public async Task<ActionResult<IEnumerable<UserBorrowRequestDto>>> GetPendingRequests(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return BadRequest("User not found");
            string id = user.Id;
            var borrowRequests = await _borrowRequestService.GetAll();
            var ret = borrowRequests.Where(b => b.UserId == id && b.Status == "PENDING");
            List<UserBorrowRequestDto> lst = new List<UserBorrowRequestDto>();
            foreach (var b in ret)
            {
                lst.Add(new UserBorrowRequestDto(b));
            }
            return Ok(lst);
        }
        
        [HttpGet("requests-log/{userName}")]
        public async Task<ActionResult<IEnumerable<BorrowRequestDto>>> GetAllRequests(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
                return BadRequest("User not found");
            string id = user.Id;
            var borrowRequests = await _borrowRequestService.GetAll();
            var ret = borrowRequests.Where(b => b.UserId == id);
            List<BorrowRequestDto> lst = new List<BorrowRequestDto>();
            foreach (var b in ret)
            {
                lst.Add(new BorrowRequestDto(b));
            }
            return Ok(lst);
        }

        [HttpPut("return-book/{requestID}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> updateToReturned(int requestID)
        {
            var request = await _borrowRequestService.GetById(requestID);
            if (request == null)
            {
                return BadRequest("ID not found");
            }
            request.Status = "RETURNED";
            request.ReturnDate = DateTime.Now;
            await _borrowRequestService.Update(request);
            return NoContent();
        }
    }
}
