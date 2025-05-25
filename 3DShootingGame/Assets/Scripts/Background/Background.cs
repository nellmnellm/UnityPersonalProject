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
        // ������ �ӵ� ��� (0.05�� ���� ���, �������� �� �ε巯��)
        smoothedSpeed = Mathf.Lerp(smoothedSpeed, targetSpeed, 0.05f);

        // ���� ��� �� ������ ����
        currentOffset += Vector2.up * smoothedSpeed * Time.deltaTime;

        // ����
        backgroundMaterial.mainTextureOffset = currentOffset;
    }

    [ContextMenu("�ؽ��� ����")]
    public void TextureChange()
    {
        backgroundMaterial.SetTexture("_BaseMap", newTexture); // Basemap = urp �⺻ ���̴� �Ӽ��̸�.
    }
}
