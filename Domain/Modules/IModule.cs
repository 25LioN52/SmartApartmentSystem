namespace Domain.Modules
{
    public interface IModule
    {
        bool WasPingedForLastPeriod();
        bool Ping();
        bool Reset();
    }
}
