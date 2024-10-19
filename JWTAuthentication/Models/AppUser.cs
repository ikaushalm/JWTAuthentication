using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

public class AppUser:IdentityUser
{
    [PersonalData]
    [Column(TypeName= "VARCHAR(50)")]
    public string FullName { get; set; }
}