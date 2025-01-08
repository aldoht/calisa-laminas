using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("Payments")]
public class Payment : BaseModel
{
    [PrimaryKey("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("method")]
    public required string Method { get; set; }

    [Column("bank")]
    public string? Bank { get; set; }

    [Column("created_at")]
    public DateTimeOffset? CreatedAt { get; set; }

    [Column("user_id")]
    public required string UserId { get; set; }
}