using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BattleTurn
{
    Start, 
    Turn1, Turn2, 
    Action1, Action2,
    Win, Lose
}

public enum UI_State
{
    MainMenu, SkillMenu
}
public class BattleSystem : MonoBehaviour
{
    public BattleTurn state;
    public UI_State uiState;

    public TMP_Text PhaseText;
    
    public Unit playerUnit;
    public Unit otherUnit;

    public Button[] gameButtons;
    public Button[] skillButtons;
    public GameObject mainMenuPanel;
    public GameObject skillMenuPanel;
    private int mainMenuIdx = 0;
    private int skillMenuIdx = 0;
    public UnitHPBar playerBar;
    public UnitHPBar otherBar;

    private void Start()
    {
        state = BattleTurn.Start;

        playerBar.SetSliderBar(playerUnit);
        otherBar.SetSliderBar(otherUnit);
        StartCoroutine(PlayerTurn());
        //OpenMainMenu();

       
    }

    private void Update()
    {
        switch(uiState)
        {
            case UI_State.MainMenu:
                {
                    HandleMainMenuInput();
                    break;
                }
            case UI_State.SkillMenu:
                {
                    HandleSkillMenuInput();
                    break;
                }
               
                
        }
    }

    public IEnumerator PlayerAttack(Skill skill)
    {
        yield return new WaitForSeconds(1f);
        otherUnit.currentHP -= skill.damage;
        otherBar.UpdateHP(otherUnit.currentHP);
        yield return StartCoroutine(EnemyTurn());

    }

    public IEnumerator EnemyAttack(Skill skill)
    {
        yield return new WaitForSeconds(1f);
        playerUnit.currentHP -= skill.damage;
        playerBar.UpdateHP(playerUnit.currentHP);
        yield return StartCoroutine(PlayerTurn());

    }
    public IEnumerator PlayerTurn()
    {
        yield return StartCoroutine(Phase("Player's turn"));
        OpenMainMenu();
        
    }

    public IEnumerator EnemyTurn()
    {
        yield return StartCoroutine(Phase("Enemy's turn"));

        Debug.Log($"{otherUnit.unitName}의 {otherUnit.skills[0].skillName}!!");

        yield return StartCoroutine(EnemyAttack(otherUnit.skills[0]));
        //적이 진행할 행동 구현
        //yield return StartCoroutine(PlayerTurn());
    }


    public IEnumerator Phase(string message, float duration = 1.5f)
    {
        PhaseText.text = message;
        PhaseText.gameObject.SetActive(true);

        yield return new WaitForSeconds(duration);

        PhaseText.gameObject.SetActive(false);  
    }

    private void OpenMainMenu()
    {
        mainMenuPanel.SetActive(true);
        skillMenuPanel.SetActive(false);
        uiState = UI_State.MainMenu;
        mainMenuIdx = 0;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameButtons[mainMenuIdx].gameObject);
    }

    private void HandleMainMenuInput()
    {

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            mainMenuIdx = (mainMenuIdx + 1) % gameButtons.Length;
            EventSystem.current.SetSelectedGameObject(gameButtons[mainMenuIdx].gameObject);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            mainMenuIdx = (mainMenuIdx + 3) % gameButtons.Length;
            EventSystem.current.SetSelectedGameObject(gameButtons[mainMenuIdx].gameObject);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            mainMenuIdx = (mainMenuIdx + 2) % gameButtons.Length;
            EventSystem.current.SetSelectedGameObject(gameButtons[mainMenuIdx].gameObject);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            mainMenuIdx = (mainMenuIdx + 2) % gameButtons.Length;
            EventSystem.current.SetSelectedGameObject(gameButtons[mainMenuIdx].gameObject);
        }


        if (Input.GetKeyDown(KeyCode.Return))
        {
            gameButtons[mainMenuIdx].onClick.Invoke();
/*
            switch (mainMenuIdx)
            {
                case 0:
                    OpenSkillMenu(); break;
                case 1:
                    OpenBagMenu(); break;
                case 2:
                    SelectPokemonMenu(); break;
                case 3:
                    Run(); break;
            }*/

        }
    }

    public void OpenSkillMenu()
    {
        mainMenuPanel.SetActive(false);
        skillMenuPanel.SetActive(true);
        uiState = UI_State.SkillMenu;
        skillMenuIdx = 0;

        for (int i = 0; i < skillButtons.Length; i++)
        {
            if (i < playerUnit.skills.Length)
            {
                skillButtons[i].gameObject.SetActive(true);
                skillButtons[i].GetComponentInChildren<TMP_Text>().text
                    = playerUnit.skills[i].skillName;
            }
            else
            {
                skillButtons[i].gameObject.SetActive(false);
            }
        }
        EventSystem.current.SetSelectedGameObject(null);
        
        EventSystem.current.SetSelectedGameObject(gameButtons[skillMenuIdx].gameObject);

    }


    private void HandleSkillMenuInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            skillMenuIdx = (skillMenuIdx + 1) % skillButtons.Length;
            EventSystem.current.SetSelectedGameObject(skillButtons[skillMenuIdx].gameObject);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            skillMenuIdx = (skillMenuIdx + 3) % skillButtons.Length;
            EventSystem.current.SetSelectedGameObject(skillButtons[skillMenuIdx].gameObject);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            /*Skill playerSelected = playerUnit.skills[skillMenuIdx];
            Debug.Log($"{playerUnit.unitName} 의 {playerSelected.skillName} ! ! !");
            StartCoroutine(PlayerAttack(playerUnit.skills[skillMenuIdx]));*/
            OnSkillButtonPressed(skillMenuIdx);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OpenMainMenu();
        }
    }

    public void OnSkillButtonPressed(int index)
    {
        Skill playerSelected = playerUnit.skills[index];
        Debug.Log($"{playerUnit.unitName} 의 {playerSelected.skillName}!!!");

        StartCoroutine(PlayerAttack(playerSelected));
    }





    public void OpenBagMenu()
    {
        Debug.Log("가방");
    }

    public void SelectPokemonMenu()
    {
        Debug.Log("선택");
    }

    public void Run()
    {
        Debug.Log("달림");
    }

   
}
