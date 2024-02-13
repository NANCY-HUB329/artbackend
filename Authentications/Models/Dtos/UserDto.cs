﻿using System.ComponentModel.DataAnnotations;

namespace Authentications.Models.Dtos
{
    

   
        public class UserDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; } = string.Empty;


            public string Email { get; set; } = string.Empty;

            public string PhoneNumber { get; set; } = string.Empty;
        }
    }