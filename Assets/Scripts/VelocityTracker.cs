using UnityEngine;

public class VelocityTracker : MonoBehaviour
{
    public Vector3 Velocity { get; private set; }
    public float Speed => Velocity.magnitude;

    private Vector3 previousPosition;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Velocity = (transform.position - previousPosition) / Time.fixedDeltaTime;
        previousPosition = transform.position;
    }
}
