using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;

namespace Assets.Scripts.Manager
{
    public class TileManager : MonoBehaviour
    {
        //상호작용 맵에 대한 설정
        [SerializeField] private Tilemap interactables;

        //타일(상호작용 이미지 가리기)
        [SerializeField] private Tile hidden;
        //타일(상호작용)
        [SerializeField] private Tile interacted;
        //타일(심어진 위치)
        [SerializeField] private Tile grown;

        private void Start()
        {
            foreach (var pos in interactables.cellBounds.allPositionsWithin)
            {
                //TileBase는 타일 클래스의 기초 클래스
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
                //현재 interactable로 설정한 타일의 이름을 배치합니다.
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