using Event_Management.Domain;
using Event_Management.Domain.Repository;
using Event_Management.Domain.Repository.Common;
using Event_Management.Domain.UnitOfWork;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Repository.SQL;

namespace Event_Management.Infrastructure.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
    {
        private readonly EventManagementContext _context;

        private readonly ICacheRepository _cacheRepository;
		private readonly IEventMailRepository _eventMailRepository = null!;
        
        private readonly IEventRepository _eventRepository = null!;
        private readonly IFeedbackRepository _feedbackRepository = null!;
        private readonly ILogoRepository _logoRepository = null!;
        private readonly IParticipantRepository _participantRepository = null!;
        
        
        private readonly IPaymentTransactionRepository _paymentTransactionRepository = null!;
        private readonly ISponsorEventRepository _sponsorEventRepository = null!;
        
        private readonly IRoleEventRepository _roleEventRepository = null!;
        private readonly IRoleRepository _roleRepository = null!;
        
        private readonly ITagRepository _tagRepository = null!;
        private readonly IPaymentTransactionRepository _transactionRepository = null!;
        private readonly IUserRepository _userRepository = null!;
        private readonly IUserValidationRepository _userValidationRepository = null!;
        private readonly IRefreshTokenRepository _refreshTokenRepository = null!;
        public UnitOfWork()
        {
        }
        public UnitOfWork(EventManagementContext context, ICacheRepository cacheRepository)
        {
			_context = context;
			_cacheRepository = cacheRepository;
		}

        public UnitOfWork(ICacheRepository cacheRepository)
        {
            _context = new EventManagementContext();
			_cacheRepository = cacheRepository;
		}


        public IEventMailRepository EventMailRepository => _eventMailRepository ?? new EventMailRepository(_context);

        //public IEventPaymentRepository EventPaymentRepository => _eventPaymentRepository ?? new EventPaymentRepository(_context);

        public IEventRepository EventRepository => _eventRepository ?? new EventRepository(_context);

        public IFeedbackRepository FeedbackRepository => _feedbackRepository ?? new FeedbackRepository(_context);

        public ILogoRepository LogoRepository => _logoRepository ?? new LogoRepository(_context);

        public IParticipantRepository ParticipantRepository => _participantRepository ?? new ParticipantRepository(_context, _cacheRepository);

        

       
        public IPaymentTransactionRepository PaymentTransactionRepository => _paymentTransactionRepository ?? new PaymentTransactionRepository(_context);

        public ISponsorEventRepository SponsorEventRepository => _sponsorEventRepository ?? new SponsorEventRepository(_context);

       

        public IRoleEventRepository RoleEventRepository => _roleEventRepository ?? new RoleEventRepository(_context);

        public IRoleRepository RoleRepository => _roleRepository ?? new RoleRepository(_context);

        

        public ITagRepository TagRepository => _tagRepository ?? new TagRepository(_context);

        public IPaymentTransactionRepository TransactionRepository => _transactionRepository ?? new PaymentTransactionRepository(_context);

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);
        public IUserValidationRepository UserValidationRepository => _userValidationRepository ?? new UserValidationRepository(_context);
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ?? new RefreshTokenRepository(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
