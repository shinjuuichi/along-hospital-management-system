namespace SharedLibrary.Commons.EntityAnnotations
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CollectionNameAttribute(string name) : Attribute
    {
        public string Name { get; } = name;
    }
}