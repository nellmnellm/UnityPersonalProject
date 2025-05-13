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
    public Transform[] cameraPositions; // ī�޶� �̵��� ��ġ��
    public CharacterInfo[] characters; // �� ��ġ�� ĳ���� (info ���ٿ�)
    public GameObject[] playerPrefabs; // ĳ���� ���ÿ� ���� �÷��̾� ������
    public GameObject[] timelineObjects;// ĳ���ͺ� �ִϸ����� Ÿ�Ӷ��� ����
    public Transform cameraTransform;   // ī�޶� ����

    private int currentIndex = 0;

    public Button leftButton;
    public Button rightButton;



    private Vector3 targetPosition;

    private void Start()
    {
        MoveToCharacter(0); // ���� �� ù ĳ���ͷ� �̵�
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