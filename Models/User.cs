﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace F_LocalBrand.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public string Address { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public int? Otp { get; set; }

    public int? RoleId { get; set; }

    public virtual Role Role { get; set; }
}