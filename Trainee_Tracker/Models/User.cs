// Code Owner: Jelena Cosic
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Trainee_Tracker.Models
{
    public abstract class User
    {
        public int Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty; 
        public bool Closed { get; set; }

        public void UpdatePassword(string newPassword)
        {
            HashedPassword = newPassword;
        }
    }
}