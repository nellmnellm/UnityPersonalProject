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

    [ContextMenu("텍스쳐 변경")]
    public void TextureChange()
    {
        backgroundMaterial.SetTexture("_BaseMap", newTexture); // Basemap = urp 기본 셰이더 속성이름.
    }
}
