using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace laminas_calisa.Models
{
    [Table("Clients")]
    public class Client : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; } = string.Empty;

        [Column("last_name")]
        public string LastName { get; set; } = string.Empty;

        [Column("alias")]
        public string? Alias { get; set; }

        [Column("notes")]
        public string? Notes { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("phone_number")]
        public string? PhoneNumber { get; set; }

        [Column("modified_at")]
        public DateTimeOffset ModifiedAt { get; set ; } = DateTimeOffset.UtcNow;

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}