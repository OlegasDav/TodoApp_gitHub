using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApi.Models.ResponseModels
{
    public class UserResponse
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
