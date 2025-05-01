﻿namespace FocusFlow.WebApi.DTOs
{
    public record RegisterDto(string Username, string Email, string Password);
    public record LoginDto(string Username, string Password);
}