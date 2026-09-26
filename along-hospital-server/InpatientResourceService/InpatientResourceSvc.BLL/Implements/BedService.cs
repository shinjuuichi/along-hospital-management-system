using AutoMapper;
using InpatientResourceSvc.BLL.DTOs.BedDTOs;
using InpatientResourceSvc.BLL.Interfaces;
using InpatientResourceSvc.DAL.Models;
using SharedLibrary.Base.Data;
using SharedLibrary.Base.Data.SqlServerDb;
using SharedLibrary.Base.Services;
using SharedLibrary.Commons.Exceptions;

namespace InpatientResourceSvc.BLL.Implements
{
    public class BedService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
        : BaseService<Bed, UpsertBedDTO, UpsertBedDTO, GetBedDTO>(unitOfWork, mapper, includes: [nameof(Bed.BedCategory)]),
            IBedService
    {
        private readonly IGenericRepository<Room> _roomRepository = unitOfWork.Repository<Room>();

        public override async Task<GetBedDTO> CreateAsync(UpsertBedDTO createDTO)
        {
            var room = await _roomRepository.GetByIdAsync(createDTO.RoomId)
                ?? throw new DataNotFoundException(typeof(Room), createDTO.RoomId);

            createDTO.Code = await this.GenerateBedCodeAsync(room.Code, createDTO.RoomId);

            return await base.CreateAsync(createDTO);
        }

        /*
         * Pattern: {RoomCode}-{Number:D2}
         * Example: R01-01, R01-02, R02-01
         */
        private async Task<string> GenerateBedCodeAsync(string roomCode, int roomId)
        {
            // Get all existing beds in room
            var existingBeds = await _repository.GetAllAsync(b => b.RoomId == roomId);

            // Find max bed number
            int maxBedNumber = existingBeds
                .Select(b => int.TryParse(b.Code.Split('-').LastOrDefault(), out int num) ? num : 0)
                .DefaultIfEmpty(0)
                .Max();

            int nextBedNumber = maxBedNumber + 1;
            string generatedCode;

            // Ensure uniqueness
            do
            {
                generatedCode = $"{roomCode}-{nextBedNumber:D2}";
                var existingBed = await _repository.GetByConditionAsync(b => b.Code == generatedCode);
                if (existingBed == null)
                {
                    break;
                }

                nextBedNumber++;
            } while (true);

            return generatedCode;
        }
    }
}