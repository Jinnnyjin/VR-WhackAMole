using UnityEngine;
using UnityEngine.Pool;

public class MolePool : MonoBehaviour
{
    [SerializeField] private GameObject molePrefab;
    [SerializeField] private Transform moleContainer;

    private ObjectPool<GameObject> pool;

    private void Awake()
    {
        pool = new ObjectPool<GameObject>(
            createFunc: CreateMole,
            actionOnGet: OnGetMole,
            actionOnRelease: OnReleaseMole,
            actionOnDestroy: OnDestroyMole,
            collectionCheck: true,
            defaultCapacity: 6,
            maxSize: 6);
    }

    public GameObject Get()
    {
        return pool.Get();
    }

    public void Release(GameObject mole)
    {
        pool.Release(mole);
    }

    private GameObject CreateMole()
    {
        return Instantiate(molePrefab, moleContainer);
    }

    private void OnGetMole(GameObject mole)
    {
        mole.SetActive(true);
    }

    private void OnReleaseMole(GameObject mole)
    {
        mole.SetActive(false);
    }

    private void OnDestroyMole(GameObject mole)
    {
        Destroy(mole);
    }
}
