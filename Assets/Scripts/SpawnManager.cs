using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> holes;
    [SerializeField] private MolePool molePool;
    [SerializeField] private float spawnInterval = 1.5f;
    [SerializeField] private float moleVisibleDuration = 1f;

    private List<Transform> availableHoles;

    private void Awake()
    {
        availableHoles = new List<Transform>(holes);
    }

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            TrySpawnMole();
        }
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

        GameObject mole = molePool.Get();
        mole.transform.SetPositionAndRotation(hole.position, hole.rotation);

        StartCoroutine(DespawnAfterDelay(mole, hole));
    }

    private IEnumerator DespawnAfterDelay(GameObject mole, Transform hole)
    {
        // Visible Duration동안 대기
        yield return new WaitForSeconds(moleVisibleDuration);

        // 사용 후 릴리즈, Hole 가능한 리스트에 추가
        molePool.Release(mole);
        availableHoles.Add(hole);
    }
}
