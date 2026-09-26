using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;

namespace MedicineSvc.DAL.Models
{
    [PrimaryKey(nameof(MedicineSKUId), nameof(OptionValueId))]
    public class SKUValue : Entity
    {
        public int MedicineSKUId { get; set; }

        public int OptionValueId { get; set; }

        public virtual MedicineSKU? MedicineSKU { get; set; }
        public virtual OptionValue? OptionValue { get; set; }
    }
}