using UnityEngine;

public class PlayerHighlight : MonoBehaviour
{
    public GameObject highlightObject; // 자식 오브젝트로 등록

    private void Start()
    {
        Material glowMat = highlightObject.GetComponent<Renderer>().material;
        glowMat.renderQueue = 3100; // Transparent보다 더 뒤에 렌더링
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            highlightObject.SetActive(true);
        }
        else
        {
            highlightObject.SetActive(false);
        }
    }
}