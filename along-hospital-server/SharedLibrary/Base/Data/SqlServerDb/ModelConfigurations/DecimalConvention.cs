using Microsoft.EntityFrameworkCore;

namespace SharedLibrary.Base.Data.SqlServerDb.ModelConfigurations
{
    public static class DecimalConvention
    {
        public static void ConfigureDecimalConvention(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<double>().HaveColumnType("numeric(18, 2)");
        }
    }
}
