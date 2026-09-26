using Microsoft.EntityFrameworkCore;
using SharedLibrary.Commons.EntityAbstractions;
using SharedLibrary.Commons.EntityAnnotations.OnDeleteAttributes;
using System.ComponentModel.DataAnnotations;

namespace InpatientResourceSvc.DAL.Models
{
    [PrimaryKey(nameof(RoomCategoryRoleId), nameof(RoomCategoryId))]
    public class RoomCategoryRoleMapping : Entity
    {
        public int RoomCategoryRoleId { get; set; }

        public int RoomCategoryId { get; set; }

        [OnDelete(OnDeleteBehavior.Cascade)]
        public virtual RoomCategoryRole? RoomCategoryRole { get; set; }

        public virtual RoomCategory? RoomCategory { get; set; }
    }
}
