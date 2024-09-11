using GestionTurnosApis.Context;
using GestionTurnosApis.Models;
using GestionTurnosApis.Models.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace GestionTurnosApis.Services
{
    public class ShiftService : IShiftService
    {
        private readonly ApplicationDbContext _context;

        public ShiftService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<ShiftDto>> GetAvailableShifts()
        {
            var currentDateTime = DateTime.Now;
            var availableShifts = await (from shift in _context.Shift
                                         join branch in _context.Branch
                                         on shift.IdBranch equals branch.IdBranch
                                         where shift.IdUser == null && shift.ExpirationTime > currentDateTime
                                         select new ShiftDto
                                         {
                                             IdShift = shift.IdShift,
                                             BranchName = branch.Name,
                                             BranchAddress = branch.Address,
                                             ScheduledDateTime = shift.ScheduledDateTime,
                                             IsActive = shift.IsActive
                                         })
                                         .ToListAsync();
            return availableShifts;
        }

        public async Task<List<ShiftDto>> GetShiftsByUser(int idUser)
        {
            var currentDateTime = DateTime.Now;
            var userShifts = await (from shift in _context.Shift
                                    join branch in _context.Branch
                                    on shift.IdBranch equals branch.IdBranch
                                    where shift.IdUser == idUser && shift.ExpirationTime > currentDateTime
                                    select new ShiftDto
                                    {
                                        IdShift = shift.IdShift,
                                        BranchName = branch.Name,
                                        BranchAddress = branch.Address,
                                        ScheduledDateTime = shift.ScheduledDateTime,
                                        ExpirationTime = shift.ExpirationTime,
                                        IsActive = shift.IsActive,
                                        DateAssociation = shift.DateAssociation
                                    })
                                    .ToListAsync();
            return userShifts;
        }

        public async Task<ShiftDto> GetShiftById(int idShift)
        {
            var shiftDetail = await (from shift in _context.Shift
                               join branch in _context.Branch
                               on shift.IdBranch equals branch.IdBranch
                               join user in _context.User
                               on shift.IdUser equals user.IdUser into userGroup
                               from user in userGroup.DefaultIfEmpty()
                               where shift.IdShift == idShift
                               select new ShiftDto
                               {
                                   IdShift = shift.IdShift,
                                   UserDocument = user != null ? user.DocumentNumber : null,
                                   BranchName = branch.Name,
                                   BranchAddress = branch.Address,
                                   ScheduledDateTime = shift.ScheduledDateTime,
                                   ExpirationTime = shift.ExpirationTime,
                                   IsActive = shift.IsActive,
                                   DateAssociation = shift.DateAssociation
                               })
                               .FirstOrDefaultAsync();
            return shiftDetail;
        }

        public async Task<Shift> CreateShift(int idBranch, DateTime scheduledDateTime)
        {
            Shift shift = new Shift();
            shift.IdBranch = idBranch;
            shift.ScheduledDateTime = scheduledDateTime;
            shift.ExpirationTime = shift.ScheduledDateTime.AddMinutes(15);
            shift.IsActive = false;
            _context.Shift.Add(shift);
            await _context.SaveChangesAsync();
            return shift;
        }

        public async Task<string> AssignUserToShift(int idShift, int idUser)
        {
            var currentDate = DateTime.Now.Date;

            var userShiftCount = await _context.Shift
                .Where(s => s.IdUser == idUser && s.DateAssociation.HasValue && s.DateAssociation.Value.Date == currentDate)
                .CountAsync();

            if (userShiftCount >= 5)
            {
                return "Has superado el tope de cinco turnos máximos al día.";
            }

            var shift = await _context.Shift.FindAsync(idShift);

            if (shift.ExpirationTime < DateTime.Now)
            {
                return "El turno ya ha expirado y no puede ser asignado.";
            }
            else
            {
                shift.IdUser = idUser;
                shift.DateAssociation = DateTime.Now;

                _context.Shift.Update(shift);
                await _context.SaveChangesAsync();

                return "Turno asignado correctamente.";
            }            
        }

        public async Task<string> UpdateShift(ShiftDto shiftDto)
        {
            var shift = await _context.Shift.FindAsync(shiftDto.IdShift);
            shift.IsActive = shiftDto.IsActive;

            _context.Shift.Update(shift);
            await _context.SaveChangesAsync();

            return "Turno actualizado correctamente.";
        }
    }
}
