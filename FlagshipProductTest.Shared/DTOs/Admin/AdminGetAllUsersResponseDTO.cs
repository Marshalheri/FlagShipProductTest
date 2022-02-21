using FlagshipProductTest.Shared.DTOs.User;
using FlagshipProductTest.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlagshipProductTest.Shared.DTOs.Admin
{
    public class AdminGetAllUsersResponseDTO
    {
        public List<AllUsersDTO> UsersDetails { get; set; }
    }

    public class AllUsersDTO : UserDTO
    {
        public int FailedLoginCount { get; set; }
        public ProfileStatus ProfileStatus { get; set; }
    }
}
