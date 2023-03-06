using Zenject;

public class StorageEvents : Installer
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<LoadFromSlot>();
        Container.DeclareSignal<GameSaved>().OptionalSubscriber();
        Container.DeclareSignal<GameLoaded>().OptionalSubscriber();
    }

    public struct LoadFromSlot
    {
        public int slot;

        public LoadFromSlot(int slot)
        {
            this.slot = slot;
        }
    }
    public struct GameSaved { }
    public struct GameLoaded { }
}
