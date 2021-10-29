using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EFCore.Sample.Api.Controllers;
using EFCore.Sample.Api.ViewModels.Safe2PayViewModel;
using EFCore.Sample.Business.Interfaces;
using EFCore.Sample.Business.Models;
using EFCore.Sample.Business.Models.Safe2Pay;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EFCore.Sample.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/safepay")]
    public class SafePayController : MainController
    {
        private readonly ISafePayService _safePayService;
        private readonly IMapper _mapper;
        public SafePayController(INotificator notificator, IMapper mapper, ISafePayService safePayService) : base(notificator)
        {
            _mapper = mapper;
            _safePayService = safePayService;
        }

        [HttpGet]
        [Route("GetByDocument:documento")]
        public async Task<IEnumerable<TransactionViewModel>> GetByDocument(string documento)
        {
            var model = _safePayService.GetClientByDocument(documento).Result?.ResponseDetail?.Objects;

            var viewModel = _mapper.Map<Business.Models.Safe2Pay.Object[], List<TransactionViewModel>>(model);

            return await Task.FromResult(viewModel);
        }
        [HttpPost]
        [Route("ReenviarNotificacao:idTransaction")]
        public async Task<ReenviarNotificacaoViewModel> ReenviarNotificacao(int idTransaction)
        {
            var model = _safePayService.ReenviarNotificacao(idTransaction).Result;

            var viewModel = _mapper.Map<ReenviarNotificacao, ReenviarNotificacaoViewModel>(model);

            return await Task.FromResult(viewModel);
        }

        [HttpPost]
        [Route("AjustaReferenciaContasReceber")]
        public async Task<IEnumerable<TransactionResponseViewModel>> AjustaReferenciaContasReceber()
        {

            var responseTransactionReference = _safePayService.AjustaReferenciaContasReceber().Result;

            var viewModel = _mapper.Map<IEnumerable<TransactionResponse>, List<TransactionResponseViewModel>>(responseTransactionReference);


            return await Task.FromResult(viewModel);
        }

        [RequestSizeLimit(40000000)]
        [HttpPost]
        [Route("ContasReceberJsonFile")]
        public async Task<ActionResult<IEnumerable<TransactionReferenceViewModel>>> ContasReceberJsonFileViewModel(IFormFile arquivo)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            ContasReceberJsonFile model = new ContasReceberJsonFile();

            model.JsonUpload = arquivo;

            if (!await UploadJsonFile(model.JsonUpload))
            {
                return CustomResponse(ModelState);
            }

            model.Json = model.JsonUpload.FileName;

            var responseTransactionsReference = _safePayService.AddContasReceberJsonFile(model).Result;

            var modelView = _mapper.Map<IEnumerable<TransactionReference>, List<TransactionReferenceViewModel>>(responseTransactionsReference);

            await RemoveUploadJsonFile(model.JsonUpload);

            return await Task.FromResult(modelView);
        }


        //[RequestSizeLimit(40000000)]
        //[DisableRequestSizeLimit]
        //[HttpPost("AdicionarJson")]
        //public ActionResult AdicionarJson(IFormFile file)
        //{
        //    return Ok(file);
        //}

        private async Task<bool> RemoveUploadJsonFile(IFormFile arquivo)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", arquivo.FileName);

            if (!System.IO.File.Exists(path)) return false;

            System.IO.File.Delete(path);
            return true;

        }
        private async Task<bool> UploadJsonFile(IFormFile arquivo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotifyError("Forneça um JsonFile para esta Rotina!");
                return false;
            }

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", arquivo.FileName);

            if (System.IO.File.Exists(path))
            {
                //System.IO.File.Delete(path);
                NotifyError("Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }



            return true;
        }


    }
}
