using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHealthUI : MonoBehaviour
{
    public PlayerHealthBar healthBarTemplate;

    private SignalBus _signalBus;
    private Player[] _players;
    private PlayerObjectsHandler _playerObjectsHandler;

    private Dictionary<Player, PlayerHealthBar> _healthBars = new Dictionary<Player, PlayerHealthBar>();

    private void Awake()
    {
        healthBarTemplate.gameObject.SetActive(false);
    }

    [Inject]
    protected void Construct(SignalBus signalBus, Player[] players, PlayerObjectsHandler handler)
    {
        _signalBus = signalBus;
        _players = players;
        _playerObjectsHandler = handler;
    }

    private void Update()
    {
        var activeIndex = _playerObjectsHandler.GetActiveIndex();
        for (int i = 0; i < _players.Length; i++)
            _healthBars[_players[i]].activeBackground.enabled = activeIndex == i;
    }

    public void Start()
    {
        int index = 0;
        foreach (var player in _players)
        {
            _healthBars.Add(player, CreateHealthBar(player, index++));
            UpdateHealth(player);
        }

        _signalBus.Subscribe<GameEvents.CharacterDamaged>(OnPlayerDamaged);
    }

    public void OnDestroy()
    {
        _signalBus.Unsubscribe<GameEvents.CharacterDamaged>(OnPlayerDamaged);
    }

    private void OnPlayerDamaged(GameEvents.CharacterDamaged data)
    {
        UpdateHealth(data.player);
    }

    private void UpdateHealth(Player player)
    {
        var bar = _healthBars[player];
        bar.health.value = player.GetHealth();
        bar.healthText.text = player.GetHealth().ToString();
    }

    private PlayerHealthBar CreateHealthBar(Player player, int index)
    {
        var healthBar = Instantiate(healthBarTemplate, healthBarTemplate.transform.parent);
        healthBar.nameText.text = player.characterName;
        healthBar.health.minValue = 0;
        healthBar.health.maxValue = player.health;

        var playerIndex = index;
        healthBar.selectButton.onClick.AddListener(() => _playerObjectsHandler.TrySelectPlayer(playerIndex));
        healthBar.gameObject.SetActive(true);
        return healthBar;
    }
}
