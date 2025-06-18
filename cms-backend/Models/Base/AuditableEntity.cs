namespace cms_backend.Models.Base
{
    public abstract class AuditableEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

        public DateTime? DeletedAt { get; set; }
        public int? DeletedBy { get; set; }
    }
}
