using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerManager : MonoBehaviour
{
    public int HP = 5;
    public int level = 1;
    public int score = 0;
    public float invincibleDuration = 2f;
    public GameObject effect;
    private bool isInvincible = false;
    private Renderer[] renderers;
    public GameObject bulletFactory;

    public GameObject[] firePosition;

   

    void Start()
    {
        // 모든 자식의 Renderer (몸이 여러 MeshRenderer로 이루어져 있을 수 있음)
        renderers = GetComponentsInChildren<Renderer>();
    }


    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            
            for (int i=0; i<level; i++)
            {
                var bullet = Instantiate(bulletFactory);
                bullet.transform.position = firePosition[i].transform.position;

            }
           
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !isInvincible)
        {
            TakeDamage(1);
        }

        else if (collision.gameObject.CompareTag("Item"))
        {
            if (level < 5)
                level++;
            else
                score += 100;
        }
    }
    void TakeDamage(int damage)
    {
        HP -= damage;

        var explosion = Instantiate(effect);
        explosion.transform.position = transform.position;

        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        float blinkInterval = 0.2f;
        float elapsed = 0f;

        while (elapsed < invincibleDuration)
        {
            SetRenderersVisible(false);
            yield return new WaitForSeconds(blinkInterval);
            SetRenderersVisible(true);
            yield return new WaitForSeconds(blinkInterval);

            elapsed += blinkInterval * 2;
        }

        isInvincible = false;
    }

    void SetRenderersVisible(bool visible)
    {
        foreach (Renderer rend in renderers)
        {
            if (rend != null)
                rend.enabled = visible;
        }
    }

}