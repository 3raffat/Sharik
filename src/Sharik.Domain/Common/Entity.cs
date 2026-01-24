namespace Sharik.Domain.Common
{
    public class Entity
    {
        public Guid Id { get; }
        protected Entity() { }

        public Entity(Guid id) {
            Id = id == Guid.Empty ? Guid.NewGuid() :id;
        }
    }
}
