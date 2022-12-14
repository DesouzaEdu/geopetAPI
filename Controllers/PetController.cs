using Microsoft.AspNetCore.Mvc;
using projetoFinal.Controllers.inputs;
using projetoFinal.Services;
using Microsoft.AspNetCore.Authorization;
using QRCoder;
using Newtonsoft.Json;
using System.Drawing;

namespace projetoFinal.Controllers;

[ApiController]
    [Route("[controller]")]
    public class PetController: ControllerBase {
        private readonly ILogger<PetController> _logger;
        private readonly PetService _service;

        public PetController(ILogger<PetController> logger, PetService service) {
            _logger = logger;
            _service = service;
        }
        ///<summary>Rota para fazer inserção de pet</summary>
        ///<response code="201"> Pet foi inserido corretamente </response>
        ///<response code="400"> Requisição no formato errado </response>
        [HttpPost(Name = "PetInput")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ResultRowstOuput))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultRowstOuput))]
        [Authorize]
        public IActionResult AddPet([FromBody] PetInput data) {
            var response = _service.CreatePet(data);
            if (response.ErrorMessage == null) {
                return StatusCode(201, response);
            }
            return StatusCode(400, response);
        }

        ///<summary>Rota para buscar todos os pets cadastrados</summary>
        ///<response code="200"> Retorna uma lista de pets </response>
        ///<response code="404">retorna um objeto com uma mensagem de erro </response>
        [HttpGet(Name = "GetAllPet")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultRowstOuput))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultRowstOuput))]
        [Authorize]
        public IActionResult GetAllPet() {
            var pets = _service.GetAll();
            if (pets == null) return StatusCode(404, new ResultRowstOuput() {
            ErrorMessage = "N�o h� pets cadastrados",
            });
            return StatusCode(200, pets);
        }

        ///<summary>Rota para buscar um pet pelo id</summary>
        ///<response code="200"> Retorna um Qrcode com dados do pet </response>
        ///<response code="404">retorna um objeto com uma mensagem de erro </response>
        [HttpGet("{id}", Name = "GetById")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultRowstOuput))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ResultRowstOuput))]
        [Authorize]
        public IActionResult GetById(int id) {
            var pet = _service.GetById(id);
            if (pet == null) return StatusCode(404, new ResultRowstOuput() {
            ErrorMessage = "Pet não encontrado",
            });
            var qrCodeGenerator = new QRCodeGenerator();
            var json = JsonConvert.SerializeObject(pet);
            var qrCodeData = qrCodeGenerator.CreateQrCode(json, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            var bitmapBytes = BitmapToBytes(qrCodeImage);

            return File(bitmapBytes, "image/jpeg");
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);

                return stream.ToArray();
            }
        }

        ///<summary>Atualiza dados do Pet</summary>
        ///<response code="400"> Encontra um problema na atualização</response>
        ///<response code="404"> Usuário não autorizado de realizar a atualização </response>
        ///<response code="204"> Atualiza com sucesso os dados </response>
        [HttpPut(Name = "UpdatePet")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultRowstOuput))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultRowstOuput))]
        [Authorize]
        public IActionResult UpdatePet(int id,  [FromBody]PetInput inputPet)
        {
            if(inputPet == null) return StatusCode(400, new ResultRowstOuput() {
                ErrorMessage = "Dados inválidos",
            });

            var result = _service.UpdatePet(id, inputPet);

            if (result.ErrorMessage == null) 
            { 
                return StatusCode(200, result);
            }
            return StatusCode(400,result);
        }
    }
