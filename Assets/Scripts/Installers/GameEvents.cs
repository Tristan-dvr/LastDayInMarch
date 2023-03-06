using Zenject;

public class GameEvents : Installer
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<PlayerEscaped>();
        Container.DeclareSignal<LevelFinished>();
        Container.DeclareSignal<PlayerDamaged>();
        Container.DeclareSignal<Pause>();
        Container.DeclareSignal<ActivePlayerChanged>();
    }

    public struct ActivePlayerChanged
    {
        public int index;
        public IControllable newActive;

        public ActivePlayerChanged(IControllable newActive, int index)
        {
            this.newActive = newActive;
            this.index = index;
        }
    }

    public struct PlayerEscaped
    {
        public ICharacter player;

        public PlayerEscaped(ICharacter player)
        {
            this.player = player;
        }
    }

    public struct LevelFinished
    {
        public bool success;

        public LevelFinished(bool success)
        {
            this.success = success;
        }
    }

    public struct PlayerDamaged
    {
        public Player player;
        public float damage;

        public PlayerDamaged(Player player, float damage)
        {
            this.player = player;
            this.damage = damage;
        }
    }

    public struct Pause
    {
        public bool pause;

        public Pause(bool pause)
        {
            this.pause = pause;
        }
    }
}
