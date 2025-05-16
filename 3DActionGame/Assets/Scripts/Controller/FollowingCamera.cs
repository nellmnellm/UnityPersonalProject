using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public float distanceAway = 4.0f;
    public float distanceUp = 2.0f;

    public Transform target;
    private void LateUpdate()
    {
        transform.position = target.position + Vector3.up * distanceUp - Vector3.forward * distanceAway;
    }
}
