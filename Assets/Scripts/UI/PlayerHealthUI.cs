using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerHealthUI : MonoBehaviour, IInitializable, IDisposable
{
    public PlayerHealthBar healthBarTemplate;

    private SignalBus _signalBus;
    private Player[] _players;
    private ActivePlayerHandler _playerObjectsHandler;

    private Dictionary<Player, PlayerHealthBar> _healthBars = new Dictionary<Player, PlayerHealthBar>();

    private void Awake()
    {
        healthBarTemplate.gameObject.SetActive(false);
    }

    [Inject]
    protected void Construct(SignalBus signalBus, Player[] players, ActivePlayerHandler handler)
    {
        _signalBus = signalBus;
        _players = players;
        _playerObjectsHandler = handler;
    }

    public void Initialize()
    {
        int index = 0;
        foreach (var player in _players)
        {
            _healthBars.Add(player, CreateHealthBar(player, index++));
            UpdateHealth(player);
        }
        UpdateActiveHealthBar();

        _signalBus.Subscribe<GameEvents.PlayerDamaged>(OnPlayerDamaged);
        _signalBus.Subscribe<GameEvents.ActivePlayerChanged>(UpdateActiveHealthBar);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<GameEvents.PlayerDamaged>(OnPlayerDamaged);
        _signalBus.Unsubscribe<GameEvents.ActivePlayerChanged>(UpdateActiveHealthBar);
    }

    private void OnPlayerDamaged(GameEvents.PlayerDamaged data)
    {
        UpdateHealth(data.player);
    }

    private void UpdateActiveHealthBar()
    {
        var activeIndex = _playerObjectsHandler.GetPlayerIndex();
        for (int i = 0; i < _players.Length; i++)
            _healthBars[_players[i]].activeBackground.enabled = activeIndex == i;
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
