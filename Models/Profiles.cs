using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("Profiles")]
public class Profile : BaseModel
{
    [PrimaryKey("id")]
    public string Id { get; set; }

    [Column("first_name")]
    public string FirstName { get; set; }

    [Column("last_name")]
    public string LastName { get; set; }

    [Column("role")]
    public string Role { get; set; }
}