namespace BeautyControl.API.Domain._Common
{
    public abstract class Entity
    {
        public virtual int Id { get; protected set; }

        protected Entity() { }

        protected Entity(int id) => Id = id;

        public bool IsTransient() => Id == default;

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not Entity)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            Entity item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;

            return item.Id.Equals(Id);
        }

        public override int GetHashCode() => GetType().GetHashCode() * 907 + Id.GetHashCode();

        public override string ToString() => $"{GetType().Name} [Id={Id}]";

        public static bool operator ==(Entity left, Entity right)
        {
            if (Equals(left, null))
                return Equals(right, null);
            else
                return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right) => !(left == right);
    }
}
