using AutoMapper;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Helper;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Models.Payment.VnpayPayment;
using Event_Management.Domain.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;

namespace Event_Management.Application.Service.Payments
{
    public class ProcessVnpayPaymentReturn : VnpayPayResponse,
        IRequest<PaymentReturnDto>
    {
    }

    public class ProcessVnpayPaymentReturnHandler : IRequestHandler<ProcessVnpayPaymentReturn, PaymentReturnDto>
    {
        
        
        private readonly VnpaySetting _vnpaySetting;
        private readonly IUnitOfWork _unitOfWork;
       
        private readonly IMapper _mapper;
        private readonly PaymentHandler _paymentHandler;

        public ProcessVnpayPaymentReturnHandler(IUnitOfWork unitOfWork, IMapper mapper,
            IOptions<VnpaySetting> vnpaySettingOptions, PaymentHandler paymentHandler)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _paymentHandler = paymentHandler;
            _vnpaySetting = vnpaySettingOptions.Value;
        }

        public async Task<PaymentReturnDto> Handle(
            ProcessVnpayPaymentReturn request, CancellationToken cancellationToken)
        {
            //string returnUrl = string.Empty;
            //var result = new PaymentReturnDto();

            var paymentReturnDto = new PaymentReturnDto();


            var payment = _paymentHandler.currentPayment;
            var isValidSignature = request.IsValidSignature(_vnpaySetting.HashSecret);

            if (isValidSignature)
            {
                paymentReturnDto.PaymentStatus = "01";
                paymentReturnDto.PaymentId = Guid.Parse(request.vnp_TxnRef);
                paymentReturnDto.RequiredAmount = request.vnp_Amount;
                paymentReturnDto.PaymentDate = payment.PaymentDate;
                paymentReturnDto.PaymentRefId = payment.PaymentRefId;

                if (payment == null)
                {
                    paymentReturnDto.PaymentStatus = "11";
                    paymentReturnDto.PaymentMessage = "Can't find payment at payment service";
                } 

                if (request.vnp_ResponseCode == "00")
                {
                    paymentReturnDto.PaymentStatus = "00";
                    paymentReturnDto.PaymentMessage = "Payment Successfully";

                   

                    ///TODO: Make signature
                    //paymentReturnDto.Signature = Guid.NewGuid().ToString();
                }
                else
                {
                    paymentReturnDto.PaymentStatus = "10";
                    paymentReturnDto.PaymentMessage = "Payment process failed";
                }
            }
            else
            {
                paymentReturnDto.PaymentStatus = "99";
                paymentReturnDto.PaymentMessage = "Invalid signature in response";
            }

            var paymentTransaction = new PaymentTransaction
            {
                Id = Guid.NewGuid(),
                TranMessage = paymentReturnDto.PaymentMessage,
                TranStatus = paymentReturnDto.PaymentStatus,
                TranAmount = request.vnp_Amount,
                TranDate = DateTime.Now,
                
            };
            await _unitOfWork.PaymentTransactionRepository.Add(paymentTransaction);

            return paymentReturnDto;
        }


    }
}
