using Event_Management.Application.Dto.PaymentDTO;
using Event_Management.Application.Helper;
using Event_Management.Domain;
using Event_Management.Domain.Entity;
using Event_Management.Domain.Model.VnpayPayment;
using Event_Management.Domain.UnitOfWork;

using MediatR;
using Microsoft.Extensions.Options;

namespace Event_Management.Application.Service.Payments.VnpayService
{
    public class PaymentHandler : IRequestHandler<PaymentDto, PaymentLinkDto>
    {
       
        private readonly VnpaySetting _vnpaySetting;
        private readonly IUnitOfWork _unitOfWork;


        public PaymentHandler( 
            IUnitOfWork unitOfWork,
            IOptions<VnpaySetting> vnpaySetting)
        {
            _unitOfWork = unitOfWork;
            _vnpaySetting = vnpaySetting.Value;
        }


        public async Task<PaymentLinkDto> Handle(PaymentDto request, CancellationToken cancellationToken)
        {
           
            var payment = new Payment
            {
                PaymentId = Guid.NewGuid(), // Tạo giá trị cho khóa chính
                PaymentContent = request.PaymentContent,
                PaymentCurrency = request.PaymentCurrency,
                PaymentRefId = request.PaymentRefId,
                RequiredAmount = request.RequiredAmount,
                PaymentDate = DateTime.Now,
                ExpireDate = DateTime.Now.AddMinutes(15),
                PaymentLanguage = request.PaymentLanguage,
                CreatedBy = request.CreatedBy,
                PaymentDestinationId = request.PaymentDestinationId,
                PaymentStatus = request.PaymentStatus, // hoặc giá trị mặc định khác nếu cần
                PaidAmount = 0, // hoặc giá trị mặc định khác nếu cần
                PaymentLastMessage = string.Empty, // hoặc giá trị mặc định khác nếu cần

            };


            await _unitOfWork.PaymentRepository.Add(payment); // Hoặc sử dụng AddAsync
            await _unitOfWork.SaveChangesAsync();

            var paymentLink = new PaymentLinkDto();
            //paymentLink.PaymentId = payment.PaymentId;
            var paymentUrl = string.Empty;

            switch (request.PaymentDestinationId)
            {
                case "VNPAY":
                    var vnpayPayRequest = new VnpayPayRequest(_vnpaySetting.Version,
                                _vnpaySetting.TmnCode,
                                DateTime.Now,
                                SystemHelper.GetIpAddress(),
                                request.RequiredAmount ?? 0,
                                request.PaymentCurrency ?? string.Empty,
                                "other",
                                request.PaymentContent ?? string.Empty,
                                _vnpaySetting.ReturnUrl,
                                payment.PaymentId.ToString());
                    paymentUrl = vnpayPayRequest.GetLink(_vnpaySetting.PaymentUrl, _vnpaySetting.HashSecret);
                    break;
                default:
                    break;
            }

            paymentLink.PaymentUrl = paymentUrl;

            return paymentLink;
        }
    }

}
