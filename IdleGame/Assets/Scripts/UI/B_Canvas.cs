using UnityEngine;

public class B_Canvas : MonoBehaviour
{
    public static B_Canvas Instance = null;

    public Transform Coin;
    public Transform Layers;

 /*  layer 1 : �����Ǵ� ������ ���� ��ġ -> CoinMove.cs
  *  layer 2 : ������ �ؽ�Ʈ ��� ��ġ -> HitText.cs
  *  layer 3 : ������ �������� ���� ��ġ -> Item_Object.cs
  */

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }//�̱���

    public Transform GetLayer(int value)
    {

        return Layers.GetChild(value);
    }


}
