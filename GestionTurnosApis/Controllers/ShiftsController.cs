using GestionTurnosApis.Models;
using GestionTurnosApis.Models.Dtos;
using GestionTurnosApis.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GestionTurnosApis.Controllers
{
    [Route("Shifts")]
    [ApiController]
    public class ShiftsController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftsController(IShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet("AvailableShifts")]
        public async Task<IActionResult> GetAvailableShifts()
        {
            var availableShifts = await _shiftService.GetAvailableShifts();

            if (availableShifts == null || !availableShifts.Any())
            {
                return NotFound("No hay turnos disponibles.");
            }

            return Ok(availableShifts);
        }

        [HttpGet("GetShiftsByUser/{idUser}")]
        public async Task<IActionResult> GetShiftsByUser(int idUser)
        {
            var userShifts = await _shiftService.GetShiftsByUser(idUser);

            if (userShifts == null || !userShifts.Any())
            {
                return NotFound($"Este usuario no cuenta con turnos activos");
            }

            return Ok(userShifts);
        }

        [HttpGet("GetShiftsById/{idShift}")]
        public async Task<IActionResult> GetShiftsById(int idShift)
        {
            var shiftDetail = await _shiftService.GetShiftById(idShift);

            if (shiftDetail == null)
            {
                return NotFound($"No se encontró el turno con Id {idShift}.");
            }

            return Ok(shiftDetail);
        }

        [HttpPost("CreateShift")]
        public async Task<IActionResult> CreateShift(int IdBranch, DateTime ScheduledDateTime)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newShift = await _shiftService.CreateShift(IdBranch, ScheduledDateTime);
            return CreatedAtAction(nameof(GetAvailableShifts), new { id = newShift.IdShift }, newShift);
        }

        [HttpPut("UpdateShift")]
        public async Task<IActionResult> UpdateShift([FromBody] ShiftDto shift)
        {
            var validateShift = await _shiftService.GetShiftById(shift.IdShift);
            if (validateShift == null)
            {
                return NotFound();
            }

            var result = await _shiftService.UpdateShift(shift);

            return Ok(result);

        }

        [HttpPut("Assign/{idShift}/{idUser}")]
        public async Task<IActionResult> AssignUserToShift(int idShift, int idUser)
        {
            var result = await _shiftService.AssignUserToShift(idShift, idUser);

            if (result == "Has superado el tope de cinco turnos máximos al día.")
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
