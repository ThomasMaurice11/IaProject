using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using api.Dtos.User;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
      [Route("api/user")]
    [ApiController]
    
     public class UserController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
      public UserController(ApplicationDBContext context)
      {
        _context = context;

      }
           [HttpGet]
           public async Task<IActionResult> GetAll(){
            var users= await _context.Users.ToListAsync();
            var userDto= users.Select(s=> s.ToUserDto());
            return Ok(users);

           }
           [HttpGet("{id}")]
           public async Task<IActionResult> GetUserById([FromRoute] int id){
            var users= await _context.Users.FindAsync(id);
            if(users==null){
            return NotFound();
            }
            return Ok(users.ToUserDto());
           }

             [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserRequestDto userDto)
        {
           

            var userModel = userDto.ToUserFromCreateDTO();
             await _context.Users.AddAsync(userModel);
            await _context.SaveChangesAsync();

             return CreatedAtAction(nameof(GetUserById), new { id = userModel.UserId }, userModel.ToUserDto());
        }
         [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update ([FromRoute] int id, [FromBody] UpdateUserRequestDto updateDto)
        { 
           

            var userModel =  await _context.Users.FirstOrDefaultAsync(x=>x.UserId ==id);

            if (userModel == null)
            {
                return NotFound();
            }
            userModel.Username=updateDto.Username ;
            userModel.Password=updateDto.Password ;
            userModel.Role=updateDto.Role ;

           await _context.SaveChangesAsync();
            return Ok(userModel.ToUserDto());
        }
          [HttpDelete]
        [Route("{id}")]
        public  async Task<IActionResult> Delete ([FromRoute] int id)
        {
           

            var userModel = await _context.Users.FirstOrDefaultAsync(x => x.UserId ==id);

            if (userModel == null)
            {
                return NotFound();
            }
           _context.Users.Remove(userModel);

           await _context.SaveChangesAsync();
            return NoContent() ;
        }
}
}