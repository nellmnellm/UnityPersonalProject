using UnityEngine;
using System.Collections;
using Assets.Scripts.InventorySystem;
using Assets.Scripts.Manager;
using Assets.Scripts.Items;

public class Player : MonoBehaviour
{
    public Inventory Inventory;
    private TileManager tileManager;

    private void Awake()
    {
        //�⺻ �κ��丮 4�� ����
        Inventory = new Inventory(4);
        tileManager = GameManager.instance.TileManager;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (tileManager != null)
            {
                var position = new Vector3Int((int)transform.position.x,
                                        (int)transform.position.y, 0);

                if (GameManager.instance.TileManager.isInteractable(position))
                {
                    Debug.Log("check");
                    GameManager.instance.TileManager.SetInteract(position);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //ä�� ���
        }
    }

    public void Drop(Item item)
    {

        //��ġ ����
        var spawn = transform.position;

        //������ ����
        float x = 5.0f;

        Vector3 offset = new Vector3(x, 0, 0);

        //������Ʈ ����
        var go = Instantiate(item, spawn + offset, Quaternion.identity);
        //������Ʈ�� ���� �������� �� �ۿ�
        //go.rbody.AddForce(offset * 2f, ForceMode2D.Impulse);

    }
}