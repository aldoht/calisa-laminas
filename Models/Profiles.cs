using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("Profiles")]
public class Profile : BaseModel
{
    [Column("id")]
    public string Id { get; set; } = string.Empty;

    [Column("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [Column("last_name")]
    public string LastName { get; set; } = string.Empty;

    [Column("role")]
    public string Role { get; set; } = string.Empty;
    
    [PrimaryKey("table_id")]
    public string TableId { get; set; } = string.Empty;
}