using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace Models;

[Table("Permissions")]
public class Permissions : BaseModel
{
    [Column("id")]
    public int Id { get; set; }

    [PrimaryKey("name")]
    public string Name { get; set; }

    [Column("createOrders")]
    public bool CreateOrders { get; set; }

    [Column("deleteOwnOrders")]
    public bool DeleteOwnOrders { get; set; }

    [Column("editOwnOrders")]
    public bool EditOwnOrders { get; set; }

    [Column("deleteOrders")]
    public bool DeleteOrders { get; set; }

    [Column("editOrders")]
    public bool EditOrders { get; set; }
}