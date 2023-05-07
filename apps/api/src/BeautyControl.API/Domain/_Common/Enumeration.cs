namespace BeautyControl.API.Domain._Common
{
    public abstract class Enumeration : IComparable
    {
        private readonly int _value;
        private readonly string _name;

        public int Value => _value;

        public string Name => _name;

        protected Enumeration() { }

        protected Enumeration(int value, string name)
        {
            _value = value;
            _name = name;
        }

        public override bool Equals(object? obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null) return false;

            var typeMatches = GetType().Equals(obj?.GetType());
            var valueMatches = _value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => _value.GetHashCode();

        public int CompareTo(object? other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));

            return Value.CompareTo(((Enumeration)other).Value);
        }
    }
}
