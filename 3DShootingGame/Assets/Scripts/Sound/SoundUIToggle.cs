using UnityEngine;

public class ToggleCanvasUI : MonoBehaviour
{
    public GameObject canvasObject; // ESC�� ����� UI ������Ʈ

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (canvasObject != null)
            {
                canvasObject.SetActive(!canvasObject.activeSelf);
            }
        }
    }

}