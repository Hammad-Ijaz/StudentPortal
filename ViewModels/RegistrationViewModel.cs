﻿namespace WebApiValidation.ViewModels
{
    public class RegistrationViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Contactno { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}