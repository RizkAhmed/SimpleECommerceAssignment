using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Business.DTOs.Auth
{
    public record LoginRequestDto(string UserName, string Password);

}
