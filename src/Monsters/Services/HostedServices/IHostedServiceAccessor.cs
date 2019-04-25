namespace Monsters.Services.HostedServices
{
    public interface IHostedServiceAccessor<T>
    {
        T Service { get; }
    }
}
