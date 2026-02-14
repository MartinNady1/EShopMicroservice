

namespace Ordering.Domain.Abstraction
{
    public interface IEntity<T> : IEntity
    {
        public T Id { get; set; }
    }
    public interface IEntity
    {
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? LastModified { get; set; }
        public string? LastModifiedBy { get; set; }
    }
}
