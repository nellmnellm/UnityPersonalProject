using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class CharacterInfo
{
    public string characterName;
    public string maxHP;
    public string Speed;
    public string skillName;

}

public class CharacterCarouselManager : MonoBehaviour
{
    public Transform[] cameraPositions; // 카메라가 이동할 위치들
    public CharacterInfo[] characters; // 각 위치의 캐릭터 (info 접근용)
    public GameObject[] playerPrefabs; // 캐릭터 선택에 따른 플레이어 프리팹
    public GameObject[] timelineObjects;// 캐릭터별 애니메이터 타임라인 적용
    public Transform cameraTransform;   // 카메라 참조

    private int currentIndex = 0;

    public Button leftButton;
    public Button rightButton;



    private Vector3 targetPosition;

    private void Start()
    {
        MoveToCharacter(0); // 시작 시 첫 캐릭터로 이동
    }

    
   


    public void MoveLeft()
    {
        if (currentIndex > 0)
        {
            if (timelineObjects[currentIndex] != null)
                timelineObjects[currentIndex].SetActive(false);

            currentIndex--;

            if (timelineObjects[currentIndex] != null)
                timelineObjects[currentIndex].SetActive(true);

            MoveToCharacter(currentIndex);
            
        }
        UpdateButtons();
    }

    public void MoveRight()
    {
        if (currentIndex < cameraPositions.Length - 1)
        {
            if (timelineObjects[currentIndex] != null)
                timelineObjects[currentIndex].SetActive(false);

            currentIndex++;

            if (timelineObjects[currentIndex] != null)
                timelineObjects[currentIndex].SetActive(true);

            MoveToCharacter(currentIndex);
        }
        UpdateButtons();
    }

    private void MoveToCharacter(int index)
    {
        cameraTransform.position = cameraPositions[index].position;
        TitleUIManager.instance.DisplayCharacterInfo(characters[index]);
    }

   
    private void UpdateButtons()
    {
        if(currentIndex == 0)
        {
            leftButton.enabled = false;
        }
        else
        {
            leftButton.enabled = true;
        }

        if (currentIndex == characters.Length - 1)
        {
            rightButton.enabled = false;
        }
        else
        {
            rightButton.enabled = true;
        }
    }

    public int GetCurrentIndex()
    {
        return currentIndex;
    }

}