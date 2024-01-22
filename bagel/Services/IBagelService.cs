namespace bagel;

public interface IBagelService
{
    Task<Bagel> GetBagelAsync(int bagelId);
    Task<IEnumerable<Bagel>> GetBagelsAsync();
}