﻿namespace Domain.Dtos.User
{
    public class UserTokenDto
    {
        public bool Authenticated { get; set; }
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}