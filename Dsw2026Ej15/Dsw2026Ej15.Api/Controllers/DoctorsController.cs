using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Dsw2026Ej15.Api.Controllers;

[Route("doctors")]
public class DoctorsController : CustomControllerBase
{
    private readonly IPersistence _persistence;
    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }
    //POST
    [HttpPost()]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            throw new ValidationException("El campo name y licenseNumber no pueden estar vacíos");
        }


        var speciality = await _persistence.GetSpecialityById(request.SpecialityId);

        if (speciality is null)
        {
            throw new ValidationException("Especialidad ingresada inexistente");
        }

        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
        await _persistence.SaveDoctor(doctor);
        Console.WriteLine("Id del Doctor: " + doctor.Id); // para tener el id a fines de prueba sin meter breakpoints
        return Created();

    }
    //GET (todos)
    [HttpGet()]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDoctors()
    {
        var activeDoctors = await _persistence.GetAllActiveDoctors();

        var responseList = activeDoctors.Select(d => new DoctorModel.Response(
            d.Name,
            d.LicenseNumber,
            d.Speciality?.Name ?? "Sin especialidad"
        ));

        return Ok(responseList);
    }

    //GET (por id)
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetDoctorById([FromRoute] Guid id)
    {
        var doctor = (await GetDoctor(id))!;

        if (doctor == null || !doctor.IsActive)
        {
            return NotFound();
        }

        var response = new DoctorModel.Response(
            doctor.Name,
            doctor.LicenseNumber,
            doctor.Speciality?.Name ?? "Sin especialidad"
            );

        return Ok(response);
    }
    
    //DELETE
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteDoctor([FromRoute] Guid id)
    {
        var doctor = (await GetDoctor(id))!;

        if (doctor == null || !doctor.IsActive)
        {
            return NotFound();
        }

        doctor.Deactivate();
        await _persistence.UpdateDoctor(doctor);
        return NoContent(); 
    }

    private async Task<Doctor?> GetDoctor(Guid id)
    {
        return await _persistence.GetDoctorById(id) ?? throw new EntityNotFoundException("Medico no encontrado");
    }
}

