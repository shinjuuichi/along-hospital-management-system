namespace SharedLibrary.Commons.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(Type entityType, int id)
            : base($"{entityType.Name} ({id}) was not found!") { }

        public DataNotFoundException(Type entityType, string id)
            : base($"{entityType.Name} ({id}) was not found!") { }

        public DataNotFoundException(string entityName, int id)
            : base($"{entityName} ({id}) was not found!") { }

        public DataNotFoundException(string? message) : base(message) { }
    }
}
