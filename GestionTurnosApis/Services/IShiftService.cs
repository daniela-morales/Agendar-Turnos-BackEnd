using GestionTurnosApis.Models;
using GestionTurnosApis.Models.Dtos;

namespace GestionTurnosApis.Services
{
    public interface IShiftService
    {
        Task<List<ShiftDto>> GetAvailableShifts();
        Task<List<ShiftDto>> GetShiftsByUser(int idUser);
        Task<ShiftDto> GetShiftById(int idShift);
        Task<Shift> CreateShift(int idBranch, DateTime scheduledDateTime);
        Task<string> UpdateShift(ShiftDto shiftDto);
        Task<string> AssignUserToShift(int idShift, int idUser);
    }
}
