using UnityEngine;

public class Background : MonoBehaviour
{
    public Material backgroundMaterial;
    public Texture2D newTexture;
    public float targetSpeed = 0.2f;

    private Vector2 currentOffset = Vector2.zero;
    private float smoothedSpeed = 0f;
    private void Start()
    {
        backgroundMaterial = new Material(backgroundMaterial);
        GetComponent<Renderer>().material = backgroundMaterial;
    }
    private void Update()
    {
        // 보간된 속도 계산 (0.05는 보간 계수, 작을수록 더 부드러움)
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, targetSpeed, 0.05f);

        // 방향 계산 및 오프셋 누적
        currentOffset += Vector2.up * smoothedSpeed * Time.deltaTime;

        // 적용
        backgroundMaterial.mainTextureOffset = currentOffset;
    }

    [ContextMenu("텍스쳐 변경")]
    public void TextureChange()
    {
        backgroundMaterial.SetTexture("_BaseMap", newTexture); // Basemap = urp 기본 셰이더 속성이름.
    }
}
