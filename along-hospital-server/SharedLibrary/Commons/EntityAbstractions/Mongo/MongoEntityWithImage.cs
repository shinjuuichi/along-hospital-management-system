namespace SharedLibrary.Commons.EntityAbstractions.Mongo
{
    public abstract class MongoEntityWithImage : MongoAuditEntity
    {
        public string? Image { get; set; }
    }
}
