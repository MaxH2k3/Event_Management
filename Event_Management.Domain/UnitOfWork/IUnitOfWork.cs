using Event_Management.Domain.Repository;

namespace Event_Management.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEventMailRepository EventMailRepository { get; }
        //IEventPaymentRepository EventPaymentRepository { get; }
        IEventRepository EventRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        ILogoRepository LogoRepository { get; }
        IParticipantRepository ParticipantRepository { get; }
       
        
        ISponsorEventRepository SponsorEventRepository { get; }
       
        IRoleEventRepository RoleEventRepository { get; }
        IRoleRepository RoleRepository { get; }
       
        ITagRepository TagRepository { get; }
        IPaymentTransactionRepository PaymentTransactionRepository { get; }
        IUserRepository UserRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; } 
        IUserValidationRepository UserValidationRepository { get; } 

		Task<bool> SaveChangesAsync();
    }
}
