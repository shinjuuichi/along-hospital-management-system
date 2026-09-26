using Microsoft.EntityFrameworkCore;
using SharedLibrary.Base.Data;
using TeleHealthSvc.DAL.Models;

namespace TeleHealthSvc.DAL.Seeds
{
    public class TeleRoomSeed : ISeedBuilder
    {
        public int Priority => 0;

        public ModelBuilder Seed(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeleRoom>().HasData(
                new TeleRoom { Id = 1, RoomCode = "TELEROOM_CARDIOLOGY", RoomDisplayName = "Cardiology TeleRoom", SpecialtyId = 1 },
                new TeleRoom { Id = 2, RoomCode = "TELEROOM_GENERAL_INTERNAL_MEDICINE", RoomDisplayName = "General Internal Medicine TeleRoom", SpecialtyId = 2 },
                new TeleRoom { Id = 3, RoomCode = "TELEROOM_OBSTETRICS_GYNECOLOGY", RoomDisplayName = "Obstetrics and Gynecology TeleRoom", SpecialtyId = 3 },
                new TeleRoom { Id = 4, RoomCode = "TELEROOM_ORTHOPEDICS", RoomDisplayName = "Orthopedics TeleRoom", SpecialtyId = 4 },
                new TeleRoom { Id = 5, RoomCode = "TELEROOM_PEDIATRICS", RoomDisplayName = "Pediatrics TeleRoom", SpecialtyId = 5 },
                new TeleRoom { Id = 6, RoomCode = "TELEROOM_DERMATOLOGY", RoomDisplayName = "Dermatology TeleRoom", SpecialtyId = 6 },
                new TeleRoom { Id = 7, RoomCode = "TELEROOM_NEUROLOGY", RoomDisplayName = "Neurology TeleRoom", SpecialtyId = 7 },
                new TeleRoom { Id = 8, RoomCode = "TELEROOM_OPHTHALMOLOGY", RoomDisplayName = "Ophthalmology TeleRoom", SpecialtyId = 8 },
                new TeleRoom { Id = 9, RoomCode = "TELEROOM_ENT", RoomDisplayName = "Otolaryngology (ENT) TeleRoom", SpecialtyId = 9 },
                new TeleRoom { Id = 10, RoomCode = "TELEROOM_UROLOGY", RoomDisplayName = "Urology TeleRoom", SpecialtyId = 10 },
                new TeleRoom { Id = 11, RoomCode = "TELEROOM_NEPHROLOGY", RoomDisplayName = "Nephrology TeleRoom", SpecialtyId = 11 },
                new TeleRoom { Id = 12, RoomCode = "TELEROOM_PULMONOLOGY", RoomDisplayName = "Pulmonology TeleRoom", SpecialtyId = 12 },
                new TeleRoom { Id = 13, RoomCode = "TELEROOM_GASTROENTEROLOGY", RoomDisplayName = "Gastroenterology TeleRoom", SpecialtyId = 13 },
                new TeleRoom { Id = 14, RoomCode = "TELEROOM_ENDOCRINOLOGY", RoomDisplayName = "Endocrinology TeleRoom", SpecialtyId = 14 },
                new TeleRoom { Id = 15, RoomCode = "TELEROOM_RHEUMATOLOGY", RoomDisplayName = "Rheumatology TeleRoom", SpecialtyId = 15 },
                new TeleRoom { Id = 16, RoomCode = "TELEROOM_HEMATOLOGY", RoomDisplayName = "Hematology TeleRoom", SpecialtyId = 16 },
                new TeleRoom { Id = 17, RoomCode = "TELEROOM_ONCOLOGY", RoomDisplayName = "Oncology TeleRoom", SpecialtyId = 17 },
                new TeleRoom { Id = 18, RoomCode = "TELEROOM_RADIOLOGY", RoomDisplayName = "Radiology TeleRoom", SpecialtyId = 18 },
                new TeleRoom { Id = 19, RoomCode = "TELEROOM_PSYCHIATRY", RoomDisplayName = "Psychiatry TeleRoom", SpecialtyId = 19 },
                new TeleRoom { Id = 20, RoomCode = "TELEROOM_REHABILITATION_MEDICINE", RoomDisplayName = "Rehabilitation Medicine TeleRoom", SpecialtyId = 20 },
                new TeleRoom { Id = 21, RoomCode = "TELEROOM_ANESTHESIOLOGY", RoomDisplayName = "Anesthesiology TeleRoom", SpecialtyId = 21 },
                new TeleRoom { Id = 22, RoomCode = "TELEROOM_EMERGENCY_MEDICINE", RoomDisplayName = "Emergency Medicine TeleRoom", SpecialtyId = 22 },
                new TeleRoom { Id = 23, RoomCode = "TELEROOM_INFECTIOUS_DISEASES", RoomDisplayName = "Infectious Diseases TeleRoom", SpecialtyId = 23 },
                new TeleRoom { Id = 24, RoomCode = "TELEROOM_FAMILY_MEDICINE", RoomDisplayName = "Family Medicine TeleRoom", SpecialtyId = 24 },
                new TeleRoom { Id = 25, RoomCode = "TELEROOM_GERIATRICS", RoomDisplayName = "Geriatrics TeleRoom", SpecialtyId = 25 }
            );
            return modelBuilder;
        }
    }
}
