

namespace Ordering.Domain.Abstraction
{
    public class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
