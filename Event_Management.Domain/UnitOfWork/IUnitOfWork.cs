using Event_Management.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event_Management.Domain.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IEventMailRepository EventMailRepository { get; }
        IEventPaymentRepository EventPaymentRepository { get; }
        IEventRepository EventRepository { get; }
        IFeedbackRepository FeedbackRepository { get; }
        IPackageRepository PackageRepository { get; }
        IParticipantRepository ParticipantRepository { get; }
        IPaymentMethodRepository PaymentMethodRepository { get; }
        IPaymentRepository PaymentRepository { get; }
        IPerkRepository PerkRepository { get; }
        IPolicyRepository PolicyRepository { get; }
        IRoleEventRepository RoleEventRepository { get; }
        IRoleRepository RoleRepository { get; }
        ISponsorMethodRepository SponsorMethodRepository { get; }
        ITagRepository TagRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        IUserRepository UserRepository { get; }

        Task<bool> SaveChangesAsync();
    }
}
