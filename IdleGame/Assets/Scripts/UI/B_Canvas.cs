using UnityEngine;

public class B_Canvas : MonoBehaviour
{
    public static B_Canvas Instance = null;

    public Transform Coin;

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
    }




}
