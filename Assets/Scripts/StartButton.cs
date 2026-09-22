using UnityEngine;

// Idle 상태(대기 화면)에서만 맞았을 때 반응하는 물리 시작 버튼
[RequireComponent(typeof(Collider))]
public class StartButton : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private float hitVelocityLimit = 1f;
    [SerializeField] private AudioClip pressSound;

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
        col.enabled = state == GameState.Idle;
    }

    private void OnTriggerEnter(Collider other)
    {
        VelocityTracker velocityTracker = other.GetComponent<VelocityTracker>();
        if (velocityTracker == null || velocityTracker.Speed < hitVelocityLimit)
            return;

        if (pressSound != null)
            AudioSource.PlayClipAtPoint(pressSound, transform.position);

        gameManager.StartGame();
    }
}
