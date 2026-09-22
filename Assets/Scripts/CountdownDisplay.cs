using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class CountdownDisplay : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        gameManager.OnCountdownTick += UpdateLabel;
        gameManager.OnGameStateChanged += HandleGameStateChanged;
        HandleGameStateChanged(gameManager.State);
    }

    private void OnDisable()
    {
        gameManager.OnCountdownTick -= UpdateLabel;
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        label.enabled = state == GameState.Countdown;
    }

    private void UpdateLabel(int secondsLeft)
    {
        label.text = secondsLeft > 0 ? secondsLeft.ToString() : "GO!";
    }
}
