using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using EFCore.Sample.Api.Controllers;
using EFCore.Sample.Api.ViewModels;
using EFCore.Sample.Business.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace EFCore.Sample.Api.V1.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/contasreceber")]
    public class ContasReceberController : MainController
    {
        private readonly IContasReceberRepository _contasReceberRepository;
        private readonly IMapper _mapper;
        public ContasReceberController(INotificator notificator, IContasReceberRepository contasReceberRepository, IMapper mapper) : base(notificator)
        {
            _contasReceberRepository = contasReceberRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("documento:documento")]
        public async Task<ActionResult<ContasReceberViewModel>> GetByDocument(string documento){

            var contasReceberViewModel = _mapper.Map<ContasReceberViewModel>(await _contasReceberRepository.GetByDocumento(documento));

            if (contasReceberViewModel == null) return NotFound();

            return contasReceberViewModel;

        }

        [HttpGet]
        public async Task<IEnumerable<ContasReceberViewModel>> GetAll()
        {

            var contasReceberViewModel = _mapper.Map<IEnumerable<ContasReceberViewModel>>(await _contasReceberRepository.GetAll());

            return contasReceberViewModel;

        }

        [HttpGet]
        public async Task<IEnumerable<ContasReceberViewModel>> GetInCacheMemory(
            [FromServices] IConfiguration config,
            [FromServices] IMemoryCache cache)
        {

            var contasReceberModel = await cache.GetOrCreateAsync<IEnumerable<ContasReceberViewModel>>(
                "ContasReceber", context =>
                {
                    context.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
                    context.SetPriority(CacheItemPriority.High);

                    return Task.FromResult((IEnumerable<ContasReceberViewModel>)_contasReceberRepository.GetAll().Result);
                });
            var contasReceberViewModel = _mapper.Map<IEnumerable<ContasReceberViewModel>>(contasReceberModel);
            
            return contasReceberViewModel;

        }


    }
}
