using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> holes;
    [SerializeField] private MolePool molePool;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private float spawnIntervalStart = 1.5f;
    [SerializeField] private float spawnIntervalEnd = 0.6f;
    [SerializeField] private float moleVisibleDurationStart = 1f;
    [SerializeField] private float moleVisibleDurationEnd = 0.5f;

    private List<Transform> availableHoles;
    private Coroutine spawnCoroutine;
    private float gameDuration;
    private float startTime;

    private void Awake()
    {
        availableHoles = new List<Transform>(holes);
    }

    // GameManager가 게임 시작 타이밍에 전체 제한시간과 함께 호출 (난이도 보간 기준)
    public void StartSpawning(float duration)
    {
        if (spawnCoroutine != null)
            return;

        gameDuration = duration;
        startTime = Time.time;
        spawnCoroutine = StartCoroutine(SpawnLoop());
    }

    public void StopSpawning()
    {
        if (spawnCoroutine == null)
            return;

        StopCoroutine(spawnCoroutine);
        spawnCoroutine = null;
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(CurrentSpawnInterval());
            TrySpawnMole();
        }
    }

    // 경과 시간 / 전체 제한시간 비율 (0~1). 게임이 진행될수록 1에 가까워짐
    private float DifficultyProgress()
    {
        if (gameDuration <= 0f)
            return 0f;

        return Mathf.Clamp01((Time.time - startTime) / gameDuration);
    }

    private float CurrentSpawnInterval()
    {
        return Mathf.Lerp(spawnIntervalStart, spawnIntervalEnd, DifficultyProgress());
    }

    private float CurrentMoleVisibleDuration()
    {
        return Mathf.Lerp(moleVisibleDurationStart, moleVisibleDurationEnd, DifficultyProgress());
    }

    private void TrySpawnMole()
    {
        if (availableHoles.Count == 0)
            return;

        // Hole 번호 구하기
        int index = Random.Range(0, availableHoles.Count);
        Transform hole = availableHoles[index];

        // 가능한 리스트에서 삭제
        availableHoles.RemoveAt(index);

        GameObject moleObject = molePool.Get();
        moleObject.transform.SetPositionAndRotation(hole.position, hole.rotation);

        Mole mole = moleObject.GetComponent<Mole>();
        mole.Setup(this, scoreManager, hole, CurrentMoleVisibleDuration());
    }

    // Mole이 맞았거나 시간 초과됐을 때 스스로 호출하는 반환 처리
    public void ReturnMole(GameObject moleObject, Transform hole)
    {
        molePool.Release(moleObject);
        availableHoles.Add(hole);
    }
}
