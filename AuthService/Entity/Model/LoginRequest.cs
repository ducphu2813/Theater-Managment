﻿namespace AuthService.Entity.Model;

public class LoginRequest
{
    public String? Username { get; set; }
    public String? Password { get; set; }
}