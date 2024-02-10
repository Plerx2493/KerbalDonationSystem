using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using KDS.Components.Account;
using Microsoft.AspNetCore.Identity;

namespace KDS.Data;

public class ApiAuth
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public ulong Id { get; set; }
    
    public string UserId { get; set; }
    public string ApiKey { get; set; }
    public DateTime CreatedAt { get; set; }
}