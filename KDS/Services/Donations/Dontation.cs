using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KDS.Services.Donations;

public class Donation
{
    [Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int Id { get; set; }
    
    //User who donated
    [Column("user")]
    public string User { get; set; }
    
    //Channel which gets the funds
    [Column("channelName")]
    public string ChannelName { get; set; }
    
    //Amount of channel points spend
    [Column("amount")]
    public int Amount { get; set; }
    
    //Creation of the donation
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }
}