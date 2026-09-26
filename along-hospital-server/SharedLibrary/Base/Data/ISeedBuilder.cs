using Microsoft.EntityFrameworkCore;

namespace SharedLibrary.Base.Data
{
    public interface ISeedBuilder
    {
        int Priority { get; }
        ModelBuilder Seed(ModelBuilder modelBuilder);
    }
}
