using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Helper;
using Event_Management.Domain.Model.VnpayPayment;
using Event_Management.Domain.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;

namespace Event_Management.Application.Service.Payments.VnpayService
{
    public class ProcessVnpayPaymentReturn : VnpayPayResponse,
        IRequest<PaymentReturnDto>
    {
    }

    public class ProcessVnpayPaymentReturnHandler : IRequestHandler<ProcessVnpayPaymentReturn, PaymentReturnDto>
    {
        
        private readonly VnpaySetting _vnpaySetting;
        private readonly IUnitOfWork _unitOfWork;

        public ProcessVnpayPaymentReturnHandler(IUnitOfWork unitOfWork,
            IOptions<VnpaySetting> vnpaySettingOptions)
        {
            _unitOfWork = unitOfWork;     
            _vnpaySetting = vnpaySettingOptions.Value;
        }

        public Task<PaymentReturnDto> Handle(
            ProcessVnpayPaymentReturn request, CancellationToken cancellationToken)
        {
            //string returnUrl = string.Empty;
            //var result = new PaymentReturnDto();

            var resultData = new PaymentReturnDto();
            resultData.PaymentStatus = "01";

           
            var isValidSignature = request.IsValidSignature(_vnpaySetting.HashSecret);

            if (isValidSignature)
            {
                var payment = _unitOfWork.PaymentRepository.GetById(request.vnp_TxnRef);

                if (payment == null)
                {
                    resultData.PaymentStatus = "11";
                    resultData.PaymentMessage = "Can't find payment at payment service";
                }

                if (request.vnp_ResponseCode == "00")
                {
                    resultData.PaymentStatus = "00";
                    resultData.PaymentId = request.vnp_TxnRef;
                    ///TODO: Make signature
                    resultData.Signature = Guid.NewGuid().ToString();
                }
                else
                {
                    resultData.PaymentStatus = "10";
                    resultData.PaymentMessage = "Payment process failed";
                }
            }
            else
            {
                resultData.PaymentStatus = "99";
                resultData.PaymentMessage = "Invalid signature in response";
            }

            return Task.FromResult(resultData);
        }


    }
}
