using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace laminas_calisa.Models
{
    [Table("Orders")]
    public class Order : BaseModel
    {
        [PrimaryKey("id")]
        public int Id { get; set; }

        [Column("client_id")]
        public int ClientId { get; set; }

        [Column("type")]
        public string Type { get; set; } = string.Empty;

        [Column("finish")]
        public string Finish { get; set; } = string.Empty;

        [Column("caliber")]
        public int Caliber { get; set; }

        [Column("length")]
        public decimal Length { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("kg")]
        public decimal Kg { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("done")]
        public bool Done { get; set; }

        [Column("paid")]
        public bool Paid { get; set; }

        [Column("user_id")]
        public string UserId { get; set; } = string.Empty;

        [Column("done_at")]
        public DateTimeOffset? DoneAt { get; set; }

        [Column("paid_at")]
        public DateTimeOffset? PaidAt { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        [Column("modified_at")]
        public DateTimeOffset ModifiedAt { get; set; } = DateTimeOffset.UtcNow;

        [Column("are_ft")]
        public bool AreFt { get; set; }
        
        [Column("is_credit")]
        public bool IsCredit { get; set; }
        
        [Column("price")]
        public decimal Price { get; set; }
    }
}