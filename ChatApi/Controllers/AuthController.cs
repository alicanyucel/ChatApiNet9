using ChatApi.Context;
using ChatApi.Dtos;
using ChatApi.Models;
using GenericFileService.Files;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace ChatApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public sealed class AuthController(ApplicationDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto request,CancellationToken cancellationToken)
        {
            bool isNameExists = await context.Users.AnyAsync(p => p.Name == request.Name,cancellationToken);
            if(isNameExists)
            {
                return BadRequest(new { Message = "bu kullanıcı ad daha önce alınmış" });
            }
            string avatar = FileService.FileSaveToServer(request.file, "wwwroot/avatar");
            User user = new()
            {
                Name = request.Name,
                Avatar = avatar

            };
            await context.AddAsync(user,cancellationToken);
            await context.SaveChangesAsync();
            return NoContent();
        }
       public async Task<IActionResult> Login(string name,CancellationToken cancellationToken)
        {
            User? user = await context.Users.FirstOrDefaultAsync(p=>p.Name == name,cancellationToken);
            if(user is null)
            {
                return BadRequest(new { Message = "kullanıcı bulunamadi" });

            }
            return Ok(new { Message = "giriş başarılı" });
        }
    }
}
