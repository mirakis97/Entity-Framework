﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace TeisterMask.Data.Models
{
    public class Employee
    {
        public Employee()
        {
            this.EmployeesTasks = new HashSet<EmployeeTask>();
        }
        [Key]
        public int Id { get; set; }
        [Required]
        public string Username { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        public ICollection<EmployeeTask> EmployeesTasks { get; set; }
    }
}