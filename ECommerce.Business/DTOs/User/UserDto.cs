using ECommerce.Business.DTOs.Product;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.DTOs.User
{
    public record UserDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime? LastLoginTime { get; set; }

        public static UserDto Map(Data.Entities.User u) => new()
        {
            Id = u.Id,
            UserName = u.UserName,
            Email = u.Email,
            LastLoginTime = u.LastLoginTime,
        };
    }
}
