using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameUI : MonoBehaviour
{
    public Button pauseButton;

    private SignalBus _signalBus;

    private void Awake()
    {
        pauseButton.onClick.AddListener(OnPausePressed);
    }

    [Inject]
    public void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnPausePressed()
    {
        _signalBus.Fire(new GameEvents.Pause(true));
    }
}
