using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }
    public float Speed => Velocity.magnitude;

    private Vector3 previousPosition;

    // 망치처럼 껐다 켰다 반복되는 오브젝트에 붙으므로, 켜질 때마다 기준 위치를 리셋해야
    // 꺼져있던 동안의 이동거리가 첫 프레임 속도에 잘못 반영되는 걸 막을 수 있음
    private void OnEnable()
    {
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = transform.position;
    }
}
