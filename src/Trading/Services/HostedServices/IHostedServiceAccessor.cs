namespace Trading.Services.HostedServices
{
    public interface IHostedServiceAccessor<T>
    {
        T Service { get; }
    }
}
