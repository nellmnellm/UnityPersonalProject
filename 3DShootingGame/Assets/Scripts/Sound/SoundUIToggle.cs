using UnityEngine;

public class ToggleCanvasUI : MonoBehaviour
{
    public GameObject canvasObject; // ESC로 토글할 UI 오브젝트

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