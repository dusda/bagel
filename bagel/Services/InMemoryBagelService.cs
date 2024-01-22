
namespace bagel;

public class InMemoryBagelService : IBagelService
{
  private readonly Dictionary<int, Bagel> _bagels = new()
  {
    [1] = new Bagel
    {
      Id = 1,
      Name = "Plain",
      Description = "The classic bagel, perfect for any occasion."
    },
    [2] = new Bagel
    {
      Id = 2,
      Name = "Everything",
      Description = "The classic bagel, topped with everything."
    },
    [3] = new Bagel
    {
      Id = 3,
      Name = "Cinnamon Raisin",
      Description = "The classic bagel, with cinnamon and raisins."
    },
    [4] = new Bagel
    {
      Id = 4,
      Name = "Blueberry",
      Description = "The classic bagel, with blueberries."
    },
    [5] = new Bagel
    {
      Id = 5,
      Name = "Egg",
      Description = "The classic bagel, with egg."
    },
    [6] = new Bagel
    {
      Id = 6,
      Name = "Onion",
      Description = "The classic bagel, with onion."
    },
    [7] = new Bagel
    {
      Id = 7,
      Name = "Poppy Seed",
      Description = "The classic bagel, with poppy seeds."
    },
    [8] = new Bagel
    {
      Id = 8,
      Name = "Pumpernickel",
      Description = "The classic bagel, with pumpernickel."
    },
    [9] = new Bagel
    {
      Id = 9,
      Name = "Salt",
      Description = "The classic bagel, with salt."
    },
    [10] = new Bagel
    {
      Id = 10,
      Name = "Sesame Seed",
      Description = "The classic bagel, with sesame seeds."
    }
  };

  public Task<Bagel> GetBagelAsync(int bagelId)
  {
    if (_bagels.TryGetValue(bagelId, out var bagel))
      return Task.FromResult(bagel);

    throw new KeyNotFoundException($"Bagel {bagelId} not found.");
  }

  public async Task<IEnumerable<Bagel>> GetBagelsAsync()
  {
    await Task.CompletedTask;
    return _bagels.Values;
  }
}
