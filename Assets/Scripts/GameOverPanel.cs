using TMPro;
using UnityEngine;

// panel(배경/텍스트를 담은 자식 오브젝트)만 켜고 끔 - 스크립트 자신의 오브젝트를 끄면 이벤트 구독도 끊겨서 다시 못 켜짐
public class GameOverPanel : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text finalScoreText;

    private void OnEnable()
    {
        gameManager.OnGameStateChanged += HandleGameStateChanged;
        HandleGameStateChanged(gameManager.State);
    }

    private void OnDisable()
    {
        gameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        bool isGameOver = state == GameState.GameOver;
        panel.SetActive(isGameOver);

        if (isGameOver)
            finalScoreText.text = $"Final Score: {scoreManager.Score}\nHit the button to restart";
    }
}
