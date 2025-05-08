using UnityEngine;

public class TestController : MonoBehaviour
{
    //스피드
    [SerializeField] private float speed = 5.0f;

    Vector3 moveVector;

    CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    private void Update()
    {
        moveVector = new Vector3( speed * Input.GetAxisRaw("Horizontal") * Time.deltaTime, 0, speed * Input.GetAxisRaw("Vertical")*Time.deltaTime);
        controller.Move(moveVector);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.transform.tag == "Obstacle")
        {
            Debug.Log("장애물이다.");
        }
    }
}
