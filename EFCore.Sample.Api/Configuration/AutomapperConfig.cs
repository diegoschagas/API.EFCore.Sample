using AutoMapper;
using EFCore.Sample.Api.ViewModels;
using EFCore.Sample.Api.ViewModels.Safe2PayViewModel;
using EFCore.Sample.Business.Models;
using EFCore.Sample.Business.Models.Safe2Pay;

namespace EFCore.Sample.Api.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            CreateMap<ContasReceber, ContasReceberViewModel>().ReverseMap();
            CreateMap<Object, TransactionViewModel>().ReverseMap();
            CreateMap<Customer, CustomerViewModel>().ReverseMap();
            CreateMap<Paymentobject, PaymentobjectViewModel>().ReverseMap();
            CreateMap<ReenviarNotificacao, ReenviarNotificacaoViewModel>().ReverseMap();
            CreateMap<TransactionResponse, TransactionResponseViewModel>().ReverseMap();
            CreateMap<ContasReceberJsonFile, ContasReceberJsonFileViewModel>().ReverseMap();
            CreateMap<TransactionReference, TransactionReferenceViewModel>().ReverseMap();

        }
    }
}