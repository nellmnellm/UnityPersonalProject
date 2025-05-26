using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Manager
{
    public class TileManager : MonoBehaviour
    {
        //��ȣ�ۿ� �ʿ� ���� ����
        [SerializeField] private Tilemap interactables;

        //Ÿ��(��ȣ�ۿ� �̹��� ������)
        [SerializeField] private Tile hidden;
        //Ÿ��(��ȣ�ۿ�)
        [SerializeField] private Tile interacted;
        //Ÿ��(�ɾ��� ��ġ)
        [SerializeField] private Tile grown;

        private void Start()
        {
            foreach (var pos in interactables.cellBounds.allPositionsWithin)
            {
                //TileBase�� Ÿ�� Ŭ������ ���� Ŭ����
                TileBase exist = interactables.GetTile(pos);

                if (exist != null)
                {
                    interactables.SetTile(pos, hidden);
                }
            }
        }

        public bool isInteractable(Vector3Int pos)
        {
            var tile = interactables.GetTile(pos);

            if (tile != null)
            {
                //���� interactable�� ������ Ÿ���� �̸��� ��ġ�մϴ�.
                if (tile.name == "hidden")
                {
                    return true;
                }
            }
            return false;
        }

        public void SetInteract(Vector3Int position)
        {
            interactables.SetTile(position, interacted);
        }

        public void SetGrown(Vector3Int position)
        {
            interactables.SetTile(position, grown);
        }

        public string GetTile(Vector3Int pos)
        {
            if (interactables != null)
            {
                var tile = interactables.GetTile(pos);

                if (tile != null)
                {
                    return tile.name;
                }
            }
            return "";
        }


    }
}