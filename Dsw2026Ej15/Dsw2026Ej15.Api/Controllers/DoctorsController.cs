using Dsw2026Ej15.Api.Models;
using Dsw2026Ej15.Domain.Entities;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;


namespace Dsw2026Ej15.Api.Controllers;

public class DoctorsController : CustomControllerBase
{
    private readonly IPersistence _persistence;
    public DoctorsController(IPersistence persistence)
    {
        _persistence = persistence;
    }
    //POST
    [HttpPost("doctors")]
    public async Task<IActionResult> CreateDoctor(DoctorModel.Request request)
    {
        if (string.IsNullOrWhiteSpace(request.Name) ||
            string.IsNullOrWhiteSpace(request.LicenseNumber))
        {
            throw new ValidationException("El campo name y licenseNumber no pueden estar vacíos");
        }
    

        var speciality = _persistence.GetSpecialityById(request.SpecialityId);

        if (speciality is null)
        {
            throw new ValidationException("Especialidad ingresada inexistente");
        }

        var doctor = new Doctor(request.Name, request.LicenseNumber, speciality);
        _persistence.SaveDoctor(doctor);

        return Created();

    }
    //GET (todos)
    [HttpGet("doctors")]
    public async Task<IActionResult> GetDoctors()
    {
        var activeDoctors = _persistence.GetAllActiveDoctors();

        var responseList = activeDoctors.Select(doctor => new DoctorModel.Response(
            doctor.Name,
            doctor.LicenseNumber,
            doctor.Speciality?.Name ?? "Sin especialidad"
        ));

        return Ok(responseList);
    }

    //GET (por id)
    [HttpGet("doctors/{id}")]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

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

    [HttpDelete("doctors/{id}")]
    //DELETE
    public async Task<IActionResult> DeleteDoctor(Guid id)
    {
        var doctor = _persistence.GetDoctorById(id);

        if (doctor == null || !doctor.IsActive)
        {
            return NotFound();
        }

        doctor.Deactivate();


        return NoContent(); 
    }
}

