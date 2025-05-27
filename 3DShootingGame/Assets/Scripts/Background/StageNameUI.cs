using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
public class StageNameUI : MonoBehaviour
{
    public float speed = 200f;
    private RectTransform rectTransform;
    public TMP_Text stageText;
    public float displayTime = 3f;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        string sceneName = SceneManager.GetActiveScene().name;
        stageText.text = sceneName;

        StartCoroutine(HideTextAfterTime(displayTime));
    }
    private void Update()
    {
        rectTransform.anchoredPosition += Vector2.up * speed * Time.deltaTime;
    }
    IEnumerator HideTextAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        stageText.gameObject.SetActive(false);
    }
}