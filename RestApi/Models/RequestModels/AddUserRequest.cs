using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models.RequestModels
{
    public class AddUserRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}
