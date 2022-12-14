using GeoPetWebApi.Controllers.inputs;
using Microsoft.AspNetCore.Mvc;
using projetoFinal.Controllers;
using projetoFinal.Services;

namespace GeoPetWebApi.Controllers {
    [ApiController]
    [Route("[controller]")]
    public class LoginController: ControllerBase {
        private readonly ILogger<PessoaCuidadoraController> _logger;
        private readonly PessoaCuidadoraService _service;

        public LoginController(ILogger<PessoaCuidadoraController> logger, PessoaCuidadoraService service) {
            _logger = logger;
            _service = service;
        }
        ///<summary>Rota para fazer login de usuário</summary>
        ///<response code="200"> retorna o token gerado</response>
        ///<response code="401">retorna um objeto com uma mensagem de erro </response>
        [HttpPost(Name = "Login")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ResultRowstOuput))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultRowstOuput))]
        public IActionResult Login([FromBody] LoginInput data) {
            var login = _service.Login(data);
            if (login.ErrorMessage == null) {
                return StatusCode(200, login);
            }
            return StatusCode(401, login);
        }
    }
}
