﻿using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
