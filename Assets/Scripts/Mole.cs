using System.Collections;
using UnityEngine;

public class Mole : MonoBehaviour
{
    [SerializeField] private float hitVelocityLimit = 1f;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitFlashDuration = 0.15f;

    private bool isResolved;
    private Coroutine timeoutCoroutine;
    private SpawnManager spawnManager;
    private Transform hole;

    private Renderer moleRenderer;
    private Color originalColor;

    private void Awake()
    {
        moleRenderer = GetComponentInChildren<Renderer>();
        originalColor = moleRenderer.material.color;
    }

    private void OnEnable()
    {
        isResolved = false;
        moleRenderer.material.color = originalColor;
    }

    public void Setup(SpawnManager manager, Transform assignedHole, float visibleDuration)
    {
        spawnManager = manager;
        hole = assignedHole;

        // 맞아서 사라질 때 타임아웃 코루틴 종료시킬 수 있도록 담아두기
        timeoutCoroutine = StartCoroutine(TimeoutRoutine(visibleDuration));
    }

    // 시간 지나면 없어지는 코루틴
    private IEnumerator TimeoutRoutine(float visibleDuration)
    {
        yield return new WaitForSeconds(visibleDuration);
        Resolve();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isResolved)
            return;

        VelocityTracker velocityTracker = other.GetComponent<VelocityTracker>();
        if (velocityTracker == null || velocityTracker.Speed < hitVelocityLimit)
            return;

        Debug.Log($"{name} hit! speed={velocityTracker.Speed:F2}");

        isResolved = true;
        if (timeoutCoroutine != null)
            StopCoroutine(timeoutCoroutine);

        StartCoroutine(FlashThenReturn());
    }

    // 맞은 순간 색 바꿨다가 잠깐 뒤에 풀 반납 (이펙트 붙이기 전 임시)
    private IEnumerator FlashThenReturn()
    {
        moleRenderer.material.color = hitColor;
        yield return new WaitForSeconds(hitFlashDuration);
        spawnManager.ReturnMole(gameObject, hole);
    }

    // 시간 초과로만 호출됨 (맞았을 때는 FlashThenReturn이 처리)
    private void Resolve()
    {
        if (isResolved)
            return;

        isResolved = true;
        spawnManager.ReturnMole(gameObject, hole);
    }
}
