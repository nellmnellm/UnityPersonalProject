using UnityEngine;

public class CameraController : MonoBehaviour
{
    Transform target; // 플레이어의 위치
    Vector3 camera_Offset; // 카메라와 플레이어 간 거리 간격
    Vector3 moveVector; //카메라의 매 프레임 이동 거리

    float transition = 0.0f; // 보간 값
    public static float camera_animate_duration = 3.0f; // 카메라 이용 에니메이션 연출 시 지속시간
    public Vector3 animate_offset = new Vector3(0, 5, 5); // 애니메이션 시작 오프셋
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        camera_Offset = transform.position - target.position; // 시작 값
    }

    private void Update()
    {
        moveVector = target.position + camera_Offset;
        moveVector.x = 0; // 카메라 x축 고정
        moveVector.y = Mathf.Clamp(moveVector.y, 3, 5);
        
        if (transition > 1.0f)
        {
            transform.position = moveVector;
        }
        else
        {
            //전환이 진행중 진행될 작업
            transform.position = Vector3.Lerp(moveVector + animate_offset, moveVector, transition);
            //플레이어의 방향까지의 보간 이동을 진행
            transition += Time.deltaTime / camera_animate_duration;
            //전환값을 증가
            transform.LookAt(target.position + Vector3.up);
            //위를 쳐다보도록
        }
    }
}
