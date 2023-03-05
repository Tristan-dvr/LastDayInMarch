using Zenject;

public class GameEvents : Installer
{
    public override void InstallBindings()
    {
        Container.DeclareSignal<PlayerEscaped>();
        Container.DeclareSignal<LevelFinished>();
        Container.DeclareSignal<CharacterDamaged>();
        Container.DeclareSignal<Pause>();
    }

    public struct PlayerEscaped
    {
        public IControllable player;

        public PlayerEscaped(IControllable player)
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

    public struct CharacterDamaged
    {
        public Player player;
        public float damage;

        public CharacterDamaged(Player player, float damage)
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
