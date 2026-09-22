using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<Transform> holes;
    [SerializeField] private MolePool molePool;
    [SerializeField] private ScoreManager scoreManager;
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

        GameObject moleObject = molePool.Get();
        moleObject.transform.SetPositionAndRotation(hole.position, hole.rotation);

        Mole mole = moleObject.GetComponent<Mole>();
        mole.Setup(this, scoreManager, hole, moleVisibleDuration);
    }

    // Mole이 맞았거나 시간 초과됐을 때 스스로 호출하는 반환 처리
    public void ReturnMole(GameObject moleObject, Transform hole)
    {
        molePool.Release(moleObject);
        availableHoles.Add(hole);
    }
}
