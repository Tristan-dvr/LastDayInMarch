using System.Threading.Tasks;

public interface IStorage
{
    GameState GetState();
    Task LoadFromSlotAsync(int slot);
    Task SaveToSlotAsync(int slot);
    GameStateMetadata GetMetadata(int slot);
    void SetMetadata(int slot, GameStateMetadata metadata);
}
