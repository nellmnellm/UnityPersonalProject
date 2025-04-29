using UnityEngine;
using UnityEngine.UI;

public class HitText : MonoBehaviour
{
    Vector3 target; //대상
    
    
    public Text message; //텍스트
    //텍스트출력 위치 보정값
    float up = 1f;

    private void Start()
    {
        
    }

    private void Update()
    {
        var pos = new Vector3(target.x, target.y + up, target.z);
        transform.position = Camera.main.WorldToScreenPoint(pos);
        up += Time.deltaTime; //시간에 따라 데미지를 올리는것
    }
    public void Init(Vector3 pos, double value)
    {
        target = pos;
        message.text = value.ToString();
        //UI를 기본 캠퍼스에 연결
        transform.parent = B_Canvas.Instance.transform;
        //
        //Release();
    }
    //일반 데미지와 크리티컬 데미지 구현

    private void Release()
    {
        Manager.Pool.pool_dict["Hit"].Release(gameObject);

    }
}
