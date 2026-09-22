using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TimerDisplay : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    private TMP_Text label;

    private void Awake()
    {
        label = GetComponent<TMP_Text>();
    }

    private void OnEnable()
    {
        gameManager.OnTimeTick += UpdateLabel;
        gameManager.OnGameStateChanged += HandleGameStateChanged;
        HandleGameStateChanged(gameManager.State);
    }

    private void OnDisable()
    {
        gameManager.OnTimeTick -= UpdateLabel;
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        label.enabled = state == GameState.Playing;
    }

    private void UpdateLabel(float secondsLeft)
    {
        label.text = $"Time: {Mathf.CeilToInt(secondsLeft)}";
    }
}
