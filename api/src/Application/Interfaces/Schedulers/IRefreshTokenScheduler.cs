namespace Application.Interfaces.Schedulers
{
    public interface IRefreshTokenScheduler
    {
        Task RemoveExpiredRefreshTokenAsync();
    }
}
