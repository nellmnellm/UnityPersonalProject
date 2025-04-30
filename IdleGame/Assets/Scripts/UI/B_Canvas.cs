using UnityEngine;

public class B_Canvas : MonoBehaviour
{
    public static B_Canvas Instance = null;

    public Transform Coin;
    public Transform Layers;

 /*  layer 1 : 생성되는 코인이 들어가는 위치 -> CoinMove.cs
  *  layer 2 : 데미지 텍스트 출력 위치 -> HitText.cs
  *  layer 3 : 생성될 아이템이 들어가는 위치 -> Item_Object.cs
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
    }//싱글톤

    public Transform GetLayer(int value)
    {

        return Layers.GetChild(value);
    }


}
