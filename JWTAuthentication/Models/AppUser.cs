﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

public class AppUser:IdentityUser
{
    [PersonalData]
    [Column(TypeName= "VARCHAR(50)")]
    public string FullName { get; set; }


    [PersonalData]
    [Column(TypeName = "nvarchar(10)")]
    public string Gender { get; set; }

    [PersonalData]
    public DateOnly DOB { get; set; }

    [PersonalData]
    public int? LibraryID { get; set; }
}