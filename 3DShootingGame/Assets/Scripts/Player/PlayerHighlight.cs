using UnityEngine;

public class PlayerHighlight : MonoBehaviour
{
    public GameObject highlightObject; // �ڽ� ������Ʈ�� ���

    private void Start()
    {
        Material glowMat = highlightObject.GetComponent<Renderer>().material;
        glowMat.renderQueue = 3100; // Transparent���� �� �ڿ� ������
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