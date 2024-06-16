using AutoMapper;
using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Helper;
using Event_Management.Application.Service.Account;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Model.VnpayPayment;
using Event_Management.Domain.UnitOfWork;
using MediatR;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace Event_Management.Application.Service.Payments.VnpayService
{
    public class ProcessVnpayPaymentIpn : VnpayPayResponse,
        IRequest<VnpayPayIpnResponse>
    {
    }

    public class ProcessVnpayPaymentIpnHandler : IRequestHandler<ProcessVnpayPaymentIpn, VnpayPayIpnResponse>
    {
        private readonly VnpaySetting _vnpaySetting;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _imapper;

        public ProcessVnpayPaymentIpnHandler(IUnitOfWork unitOfWork, IMapper imapper,
            
            ICurrentUser currentUserService,
            IOptions<VnpaySetting> vnpayConfigOptions)
        {
            _unitOfWork = unitOfWork;
            _imapper = imapper;
            _vnpaySetting = vnpayConfigOptions.Value;
        }

        public Task<VnpayPayIpnResponse> Handle(
            ProcessVnpayPaymentIpn request, CancellationToken cancellationToken)
        {
            var result = new VnpayPayIpnResponse();
            var resultData = new VnpayPayIpnResponse();


            var isValidSignature = request.IsValidSignature(_vnpaySetting.HashSecret);

            if (isValidSignature)
            {
                var paymentEntity = _unitOfWork.PaymentRepository.GetById(request.vnp_TxnRef);
                var paymentDto = _imapper.Map<PaymentDto>(paymentEntity);

                if (paymentDto != null)
                {
                    if (paymentDto.RequiredAmount == request.vnp_Amount / 100)
                    {
                        if (paymentDto.PaymentStatus != "0")
                        {
                            string message = "";
                            string status = "";

                            if (request.vnp_ResponseCode == "00" &&
                               request.vnp_TransactionStatus == "00")
                            {
                                status = "0";
                                message = "Tran success";
                            }
                            else
                            {
                                status = "-1";
                                message = "Tran error";
                            }

                            var newPaymentTransaction = new PaymentTransaction
                            {
                                Id = Guid.NewGuid(),
                                TranMessage = message,
                                TranPayload = JsonConvert.SerializeObject(request),
                                TranStatus = status,
                                TranAmount = request.vnp_Amount,
                                TranDate = DateTime.Now,
                                PaymentId = Guid.Parse(request.vnp_TxnRef),

                            };

                            _unitOfWork.PaymentTransactionRepository.Add(newPaymentTransaction);
                            var isSaved = _unitOfWork.SaveChangesAsync();
                            if (isSaved != null)
                            {
                                resultData.Set("00", "Confirm success");
                            }
                            else
                            {
                                resultData.Set("99", "Input required data");
                            }
                        }
                        else
                        {
                            resultData.Set("02", "Order already confirmed");
                        }
                    }
                    else
                    {
                        resultData.Set("04", "Invalid amount");
                    }

                }
                else
                {
                    resultData.Set("01", "Order not found");
                }
            }
            else
            {
                resultData.Set("97", "Invalid signature");
            }
            result = resultData;
            return Task.FromResult(result);
        }
    }
}
