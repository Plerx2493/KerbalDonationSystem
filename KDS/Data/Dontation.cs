using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KDS.Data;

public class Donation
{
    [Column("id"), DatabaseGenerated(DatabaseGeneratedOption.Identity), Key]
    public int Id { get; init; }
    
    /// <summary>
    /// Username of the user that donated
    /// </summary>
    [Column("user")]
    public string Username { get; set; }
    
    /// <summary>
    /// Id of the user that donated
    /// </summary>
    [Column("userId")]
    public ulong UserId { get; set; }
    
    /// <summary>
    /// Id of the channel that received the donation
    /// </summary>
    [Column("channelId")]
    public ulong ChannelId { get; set; }
    
    /// <summary>
    /// Amount of channel points donated
    /// </summary>
    [Column("amount")]
    public int Amount { get; set; }
    
    /// <summary>
    /// Value in terms of in game currency
    /// </summary>
    [Column("Value")]
    public int Value { get; set; }
    
    //Creation of the donation
    [Column("createdAt")]
    public DateTime CreatedAt { get; set; }
}