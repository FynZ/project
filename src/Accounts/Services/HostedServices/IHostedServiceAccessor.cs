namespace Accounts.Services.HostedServices
{
    public interface IHostedServiceAccessor<T>
    {
        T Service { get; }
    }
}
