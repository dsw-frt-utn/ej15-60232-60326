using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Dsw2026Ej15.Data.Dtos;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Domain.Entities;


namespace Dsw2026Ej15.Data;

public class PersistenceInMemory : IPersistence
{
    private List<Speciality> _specialities = [];
    private List<Doctor> _doctors = [];

    public PersistenceInMemory()
    {
        LoadSpecialities();
    }

    public IEnumerable<Doctor> GetAllActiveDoctors()
    {
        return _doctors.Where(d => d.IsActive).ToList();
    }

    public Doctor GetDoctorById(Guid id)
    {
        return _doctors.SingleOrDefault(d => d.Id == id);
    }

    public Speciality? GetSpecialityById(Guid id)
    {
        return _specialities.SingleOrDefault(e => e.Id == id);
    }

    public void SaveDoctor(Doctor doctor)
    {
        _doctors.Add(doctor);
    }

    private void LoadSpecialities()
    {
        try
        {
            string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                "Sources", "specialities.json");
            var json = File.ReadAllText(jsonPath);
            var specialities = JsonSerializer.Deserialize<List<SpecialityDto>>(json,
                new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            }) ?? [];
            _specialities = [.. specialities.Select(s => new Speciality(s.Name, s.Description, s.Id))];
        }
        catch(Exception)
        {
            
        }

    }

}
