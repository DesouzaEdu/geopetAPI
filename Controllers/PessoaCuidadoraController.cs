using Microsoft.AspNetCore.Mvc;
using projetoFinal.Services;
using projetoFinal.db.Models.PessoaCuidadora;
using projetoFinal.Controllers.inputs;
using GeoPetWebApi.Controllers.inputs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace projetoFinal.Controllers;

[ApiController]
[Route("[controller]")]
public class PessoaCuidadoraController : ControllerBase
{
    private readonly ILogger<PessoaCuidadoraController> _logger;
    private readonly PessoaCuidadoraService _service;

    public PessoaCuidadoraController(ILogger<PessoaCuidadoraController> logger, PessoaCuidadoraService service)
    {
        _logger = logger;
        _service = service;
    }

    ///<summary>Cria Pessoas Cuidadoras</summary>
    ///<response code="201"> retorna um objeto com a quantidade de linhas afetadas</response>
    ///<response code="400"> retorna um objeto com uma mensagem de erro</response>
    [HttpPost(Name = "CreatePessoaCuidadora")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultRowstOuput) )]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResultRowstOuput))]
    [AllowAnonymous]
    public async Task<IActionResult> CreatePessoaCuidadora(PessoaCuidadoraInput pessoaCuidadora)
    {
        var result = await _service.CreatePessoaCuidadora(pessoaCuidadora);

        if (result.ErrorMessage == null) { return StatusCode(201, result); }
        return StatusCode(400,result);

    }

    ///<summary>Retorna todas as pessoas cuidadoras cadastradas no bd</summary>
    ///<response code="200"> retorna uma lista de pesssoas cuidadoras</response>
    ///<response code="404">retorna um objeto com uma mensagem de erro </response>
    [HttpGet(Name = "GetAll")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultRowstOuput))]
    [Authorize]
    public IActionResult GetAll() {
        var list = _service.GetAll();
        if (list == null) return StatusCode(404, new ResultRowstOuput() {
            ErrorMessage = "N�o h� pessoas cadastradas",
        });
        return Ok(list);
    }

    ///<summary>Atualiza dados da pessoa cuidadora</summary>
    ///<response code="400"> Encontra um problema na atualização</response>
    ///<response code="404"> Usuário não autorizado de realizar a atualização </response>
    ///<response code="204"> Atualiza com sucesso os dados </response>
    [HttpPut(Name = "UpdatePessoaCuidadora")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultRowstOuput))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultRowstOuput))]
    // Só a propria pessoa poderá atualizar seus dados
    [Authorize]
    public IActionResult UpdatePessoaCuidadora(string email, string senha, [FromBody]PessoaCuidadoraInput inputPessoaCuidadora)
    {
        if(inputPessoaCuidadora == null) return StatusCode(400, new ResultRowstOuput() {
            ErrorMessage = "Dados inválidos",
        });
        
        if(!_service.VerifyClaimsEmailAndSenha(User, email, senha)) return StatusCode(400, new ResultRowstOuput() {
            ErrorMessage = "Email ou senha inválida",
        });

        var result = _service.UpdatePessoaCuidadora(email, inputPessoaCuidadora);

        if (result.ErrorMessage == null) 
        { 
            return StatusCode(200, result);
        }
        return StatusCode(400,result);
        
        // eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InN0cmluZyIsInNlbmhhIjoiMTIzNDU2NyIsIm5iZiI6MTY2OTYwMzE0NSwiZXhwIjoxNjY5Nzc1OTQ1LCJpYXQiOjE2Njk2MDMxNDUsImlzcyI6IkdydXBvQWxpbmVPbGl2ZWlyYUVkdWFyZG9Tb3V6YU1hcmNlbGxlTW9udGVpcm8iLCJhdWQiOiJBdmFsaWFkb3Jlc0FjZWxlcmFjYW9DU2hhcnBUcnliZSJ9.lQj02t_KadDwOTKBL8IrQRUnL6iDXGZQiDFNuEesISo
    }

    ///<summary>Atualiza status da pessoa cuidadora</summary>
    ///<response code="200"> Atualiza com sucesso o status para ativado/desativado </response>
    [HttpPatch(Name = "StatusPessoaCuidadora")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultRowstOuput))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultRowstOuput))]
    [Authorize]
    public IActionResult UpdateStatusPessoaCuidadora(string email, string senha)
    {
        if(!_service.VerifyClaimsEmailAndSenha(User, email, senha)) return StatusCode(400, new ResultRowstOuput() {
            ErrorMessage = "Email ou senha inválida",
        });

        var result = _service.UpdateStatusPessoaCuidadora(email);

        return StatusCode(200, result);
    }

    // User.Claim precisa ficar na controlar porque vem da controllerBase
    /* public bool VerifyClaimsEmailAndSenha(string email, string senha)
    {
        var emailAutorizado = User.Claims.Where(em => em.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
        var senhaAutorizada = User.Claims.Where(s => s.Type == "senha").FirstOrDefault()?.Value;

        return email == emailAutorizado && senha == senhaAutorizada;
    } */
}
