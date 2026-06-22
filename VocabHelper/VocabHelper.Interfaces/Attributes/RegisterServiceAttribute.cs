namespace VocabHelper.Interfaces
{
    public enum RegistrationType
    {
        Transient,
        Scoped,
        Singleton
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RegisterServiceAttribute<T> : RegisterServiceAttribute
    {
        public override Type InterfaceType => typeof(T);
        public RegisterServiceAttribute(RegistrationType type = RegistrationType.Transient) : base(type) { }
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class RegisterServiceAttribute : Attribute
    {
        public RegistrationType Lifetime { get; }
        public virtual Type? InterfaceType { get; } = null;
        public RegisterServiceAttribute(RegistrationType type = RegistrationType.Transient)
        {
            Lifetime = type;
        }
    }
}
