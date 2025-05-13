using UnityEngine;

public class Background : MonoBehaviour
{
    public Material backgroundMaterial;
    public Texture2D newTexture;
    public float speed = 0.2f;

    private void Update()
    {
        Vector2 dir = Vector2.up;
        backgroundMaterial.mainTextureOffset += dir * speed * Time.deltaTime;
    }

    [ContextMenu("�ؽ��� ����")]
    public void TextureChange()
    {
        backgroundMaterial.SetTexture("_BaseMap", newTexture); // Basemap = urp �⺻ ���̴� �Ӽ��̸�.
    }
}
