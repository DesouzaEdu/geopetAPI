using projetoFinal.db.Repository;
using projetoFinal.db.Models.PessoaCuidadora;
using projetoFinal.Controllers.inputs;
using projetoFinal.Controllers;
using System.Security.Cryptography;
using System.Text;
using GeoPetWebApi.Controllers.inputs;
using GeoPetWebApi.JWT;
using projetoFinal.db.Models.Pets;

namespace projetoFinal.Services
{
    public class PetService {
        private readonly PetRepository _repository;
        private readonly PessoaCuidadoraRepository _pessoaCuidadora;
        private readonly HttpClient _client;
        private readonly IConfiguration _config;

        public PetService(PetRepository repository,PessoaCuidadoraRepository pessoaCuidadoraRepository, HttpClient client, IConfiguration config) {
            _repository = repository;
            _pessoaCuidadora = pessoaCuidadoraRepository;
            _client = client;
            _config = config;
        }

        public ResultRowstOuput CreatePet(PetInput pet) {
            var output = new ResultRowstOuput();

            //verifica se o email corresponde a um usuário válido
            var pessoaCuidadora = _pessoaCuidadora.GetByEmail(pet.PessoaCuidadora);
            if(pessoaCuidadora == null || !pessoaCuidadora.Status){
                output.ErrorMessage = "Email não corresponde a um usuário válido";
                return output;
            }        

            // tudo ok? cria um pet do jeito que o bd espera 
            var model = new PetModel() {
                Nome = pet.Nome,
                Peso = pet.Peso,
                PessoaCuidadora = pessoaCuidadora,
                HashLocalizacao = GenerateHash(pet.HashLocalizacao),
                Idade = pet.Idade,
                Raca = pet.Raca,
                Porte = pet.Porte,
                Status = true,
            };
            output.RowsAffected = _repository.CreatePet(model);
            output.SucessMessage = $"Created Pet with id {model.Id}";
            
            return output;
        } 

        public string GenerateHash(string password) {
            var md5 = MD5.Create();
            byte[] bytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public IEnumerable<PetModel>? GetAll() {
            var pets = _repository.GetAll();
            return pets;
        }

        public PetModel? GetById(int id) {
            var pet = _repository.GetById(id);
            return pet;
        }

        public ResultRowstOuput UpdatePet(int id, PetInput upPet)
        {
            var output = new ResultRowstOuput();
            var pessoaCuidadora = _pessoaCuidadora.GetByEmail(upPet.PessoaCuidadora);
            if(pessoaCuidadora == null || !pessoaCuidadora.Status){
                output.ErrorMessage = "Email não corresponde a um usuário válido";
                return output;
            } 

            var updatedPet = _repository.Update(id, upPet);

            if (updatedPet == 0)
            {
                output.ErrorMessage = "Erro ao atualizar cadastro.";
                return output;
            }

            output.RowsAffected = updatedPet;
            output.SucessMessage = "Pet atualizado.";

            return output;
        }

        public ResultRowstOuput UpdateStatusPet(int id)
        {
            var output = new ResultRowstOuput();

            var newStatus = _repository.UpdateStatus(id);

            output.RowsAffected = newStatus;

            var pet = _repository.GetById(id);

            if (pet!.Status) output.SucessMessage = "Pet ativado.";
            
            else output.SucessMessage = "Pet inativado.";

            return output;
        }

    };
};
