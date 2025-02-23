﻿namespace FDA.Models
{
    // Request model for registration
    public class RegisterRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string ProfilePictureUrl { get; set; }
    }

    // Request model for login
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ModifyPasswordRequest
    {
        public int UserId { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
    public class ModifyEmailRequest
    {
        public int UserId { get; set; }
        public string NewEmail { get; set; }
        public string Password { get; set; }
    }
    public class ModifyNameRequest
    {
        public int UserId { get; set; }
        public string NewName { get; set; }
        public string Password { get; set; }
    }
}
