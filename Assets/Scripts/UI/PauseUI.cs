using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PauseUI : MonoBehaviour
{
    public GameObject pauseUI;
    public Button continueButton;

    private SignalBus _signalBus;
    private GamePauseHandler _pauseHandler;

    private void Awake()
    {
        continueButton.onClick.AddListener(OnContinuePressed);
    }

    private void OnContinuePressed()
    {
        _signalBus.Fire(new GameEvents.Pause(false));
    }

    [Inject]
    protected void Construct(SignalBus signalBus, GamePauseHandler pauseHandler)
    {
        _signalBus = signalBus;
        _pauseHandler = pauseHandler;
    }

    public void Start()
    {
        _signalBus.Subscribe<GameEvents.Pause>(UpdatePaused);
        UpdatePaused();
    }

    public void OnDestroy()
    {
        _signalBus.Unsubscribe<GameEvents.Pause>(UpdatePaused);
    }

    private void UpdatePaused()
    {
        pauseUI.SetActive(_pauseHandler.Paused);
    }
}
