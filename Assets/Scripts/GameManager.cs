using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameState
{
    Ready,
    Playing,
    GameOver
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float countdownDuration = 3f;
    [SerializeField] private float gameDuration = 60f;

    public event Action<GameState> OnGameStateChanged;
    public event Action<int> OnCountdownTick; // 남은 카운트다운 (초 단위 정수)
    public event Action<float> OnTimeTick; // 남은 플레이 시간

    public GameState State { get; private set; } = GameState.Ready;
    public float TimeRemaining { get; private set; }

    private Coroutine gameRoutine;

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        // UI 없이 재시작 흐름 검증하기 위한 임시 입력 (4번 UI 작업 후 버튼으로 대체)
        if (State == GameState.GameOver && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            StartGame();
    }

    public void StartGame()
    {
        if (gameRoutine != null)
            StopCoroutine(gameRoutine);

        gameRoutine = StartCoroutine(GameRoutine());
    }

    private IEnumerator GameRoutine()
    {
        yield return StartCoroutine(CountdownRoutine());

        scoreManager.ResetScore();
        spawnManager.StartSpawning(gameDuration);
        SetState(GameState.Playing);

        TimeRemaining = gameDuration;
        int lastLoggedSecond = -1;
        while (TimeRemaining > 0f)
        {
            OnTimeTick?.Invoke(TimeRemaining);

            int currentSecond = Mathf.CeilToInt(TimeRemaining);
            if (currentSecond != lastLoggedSecond)
            {
                Debug.Log($"[Game] Time left: {currentSecond}s");
                lastLoggedSecond = currentSecond;
            }

            yield return null;
            TimeRemaining -= Time.deltaTime;
        }

        TimeRemaining = 0f;
        OnTimeTick?.Invoke(0f);
        EndGame();
    }

    private IEnumerator CountdownRoutine()
    {
        SetState(GameState.Ready);

        int lastSecond = -1;
        float remaining = countdownDuration;
        while (remaining > 0f)
        {
            int currentSecond = Mathf.CeilToInt(remaining);
            if (currentSecond != lastSecond)
            {
                OnCountdownTick?.Invoke(currentSecond);
                Debug.Log($"[Game] Starting in {currentSecond}...");
                lastSecond = currentSecond;
            }

            yield return null;
            remaining -= Time.deltaTime;
        }

        OnCountdownTick?.Invoke(0);
    }

    private void EndGame()
    {
        spawnManager.StopSpawning();
        SetState(GameState.GameOver);
        Debug.Log($"[Game] Game Over! Final score: {scoreManager.Score} (리스타트 버튼을 치거나 에디터에서 R키로 재시작)");
    }

    private void SetState(GameState newState)
    {
        State = newState;
        OnGameStateChanged?.Invoke(newState);
    }
}
