using Microsoft.AspNetCore.Mvc;

namespace Dsw2026Ej15.Api.Controllers;

[ApiController]
[Route("api")]
public abstract class CustomControllerBase : ControllerBase
{
    //este controlador es considerado buena practica para definir los comportamientos para los demas controladores.
}
