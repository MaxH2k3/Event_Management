using Event_Management.Domain.Repository;
using Event_Management.Domain.UnitOfWork;
using Event_Management.Infrastructure.Repository.SQL;
using Event_Management.Infrastructure.DBContext;
using Event_Management.Infrastructure.Repository.SQL;

namespace Event_Management.Infrastructure.UnitOfWork
{
	public class UnitOfWork : IUnitOfWork
    {
        private readonly EventManagementContext _context;
        private readonly IEventMailRepository _eventMailRepository = null!;
        private readonly IEventPaymentRepository _eventPaymentRepository = null!;
        private readonly IEventRepository _eventRepository = null!;
        private readonly IFeedbackRepository _feedbackRepository = null!;
        private readonly IPackageRepository _packageRepository = null!;
        private readonly IParticipantRepository _participantRepository = null!;
        private readonly IPaymentMethodRepository _paymentMethodRepository = null!;
        private readonly IPaymentRepository _paymentReposiotry = null!;
        private readonly IPerkRepository _perkRepository = null!;
        private readonly IPolicyRepository _policyRepository = null!;
        private readonly IRoleEventRepository _roleEventRepository = null!;
        private readonly IRoleRepository _roleRepository = null!;
        private readonly ISponsorMethodRepository _sponsorMethodRepository = null!;
        private readonly ITagRepository _tagRepository = null!;
        private readonly ITransactionRepository _transactionRepository = null!;
        private readonly IUserRepository _userRepository = null!;

        public UnitOfWork(EventManagementContext context)
        {

            _context = context;
        }

        public UnitOfWork()
        {
            _context = new EventManagementContext();
        }


        public IEventMailRepository EventMailRepository => _eventMailRepository ?? new EventMailRepository(_context);

        public IEventPaymentRepository EventPaymentRepository => _eventPaymentRepository ?? new EventPaymentRepository(_context);

        public IEventRepository EventRepository => _eventRepository ?? new EventRepository(_context);

        public IFeedbackRepository FeedbackRepository => _feedbackRepository ?? new FeedbackRepository(_context);

        public IPackageRepository PackageRepository => _packageRepository ?? new PackageRepository(_context);

        public IParticipantRepository ParticipantRepository => _participantRepository ?? new ParticipantRepository(_context);

        public IPaymentMethodRepository PaymentMethodRepository => _paymentMethodRepository ?? new PaymentMethodRepository(_context);

        public IPaymentRepository PaymentRepository => _paymentReposiotry ?? new PaymentRepository(_context);

        public IPerkRepository PerkRepository => _perkRepository ?? new PerkRepository(_context);

        public IPolicyRepository PolicyRepository => _policyRepository ?? new PolicyRepository(_context);

        public IRoleEventRepository RoleEventRepository => _roleEventRepository ?? new RoleEventRepository(_context);

        public IRoleRepository RoleRepository => _roleRepository ?? new RoleRepository(_context);

        public ISponsorMethodRepository SponsorMethodRepository => _sponsorMethodRepository ?? new SponsorMethodRepository(_context);

        public ITagRepository TagRepository => _tagRepository ?? new TagRepository(_context);

        public ITransactionRepository TransactionRepository => _transactionRepository ?? new TransactionRepository(_context);

        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);

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
