using axAssetControl.Negocio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace axAssetControl.Controlador
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoActivoController : ControllerBase
    {
        private readonly TipoActivoN _tipoActivoNegocio;

        public TipoActivoController(TipoActivoN tipoActivoNegocio)
        {
            _tipoActivoNegocio = tipoActivoNegocio;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            var tipoActivo =  await _tipoActivoNegocio.ObtenerTipoActivo();
            if (tipoActivo == null) return NotFound(); //cod 404
            return Ok(tipoActivo); //cod 200
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            var tipoActivo = await _tipoActivoNegocio.ObtenerTipoActivoPorId(id);
            if (tipoActivo == null) return NotFound(); //cod 404
            return Ok(tipoActivo); //cod 200
        }
    }
}
