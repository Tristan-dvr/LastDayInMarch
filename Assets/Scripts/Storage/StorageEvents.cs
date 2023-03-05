using Zenject;

public class StorageEvents : Installer
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<LoadSignal>();
        Container.DeclareSignal<GameSaved>().OptionalSubscriber();
        Container.DeclareSignal<GameLoaded>().OptionalSubscriber();
    }

    public struct LoadSignal
    {
        public int slot;

        public LoadSignal(int slot)
        {
            this.slot = slot;
        }
    }
    public struct GameSaved { }
    public struct GameLoaded { }
}
