namespace Sharik.Domain.Common
{
    public class AuditableEntity : Entity
    {
        public DateTime CreatedAtUtc { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime LastModifiedUtc { get; set; }

        public string? LastModifiedBy { get; set; }

        public bool IsDeleted { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public DateTime? DeletedUtc { get; set; }

        public string? DeletedBy { get; set; }

        protected AuditableEntity()
        { }

        protected AuditableEntity(Guid id)
            : base(id)
        {
        }

    }
}
