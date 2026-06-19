namespace Dsw2026Ej15.Api.Models;

public record DoctorModel
{
    //para el verbo POST
    public record Request(string Name, string LicenseNumber, Guid SpecialityId);
    //para el verbo GET
    public record Response(string Name, string LicenseNumber, string SpecialityName);
}
