using UnityEngine;

// 게임오버 상태에서만 맞았을 때 반응하는 물리 리스타트 버튼 (두더지와 동일한 속도 기반 히트 판정 재사용)
[RequireComponent(typeof(Collider))]
public class RestartButton : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float hitVelocityLimit = 1f;

    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

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
        // 게임오버일 때만 콜라이더를 켜서 오작동 방지 (오브젝트 자체를 끄면 이 이벤트 구독도 끊기므로 콜라이더만 토글)
        col.enabled = state == GameState.GameOver;
    }

    private void OnTriggerEnter(Collider other)
    {
        VelocityTracker velocityTracker = other.GetComponent<VelocityTracker>();
        if (velocityTracker == null || velocityTracker.Speed < hitVelocityLimit)
            return;

        gameManager.StartGame();
    }
}
