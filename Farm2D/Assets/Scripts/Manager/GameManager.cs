using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Manager
{
    public class GameManager : MonoBehaviour
    {

        public ItemManager ItemManager;
        public TileManager TileManager;

        public static GameManager instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
            DontDestroyOnLoad(gameObject);

            ItemManager = GetComponent<ItemManager>();
            TileManager = GetComponent<TileManager>();  
        }



    }
}