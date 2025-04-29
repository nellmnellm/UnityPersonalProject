using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    public int hp = 50;
    public void hpPlus(int hp)
    {
        this.hp = hp;
    }
}
public class GoldenApple : MonoBehaviour
{
    
    public int value;

    public void Use()
    {
        PlayerManager.Instance.hpPlus(value);   
    }
    
    

}
