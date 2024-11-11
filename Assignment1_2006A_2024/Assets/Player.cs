using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public GameObject bloodyScreen;

    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;
    private void Start()
    {
        playerHealthUI.text = $"Health:{HP}";
    }
    public void takeDamage(int damagerAmount)
    {
        HP -= damagerAmount;

        if (HP <= 0)
        {
            print("Player dead");
            PlayerDead();
            isDead = true;
            soundManager.Instance.playerChannel.PlayOneShot(soundManager.Instance.playerDead);
        }
        else
        {
            print("player hurt");
            StartCoroutine(bloodyScreenEffect());
            playerHealthUI.text = $"Health:{HP}";
            soundManager.Instance.playerChannel.PlayOneShot(soundManager.Instance.playerHurt);
        }
    }

    private void PlayerDead()
    {
        playerHealthUI.gameObject.SetActive(false);

        GetComponent<screenBlackout>().StartFade();
        StartCoroutine(showGameOverUI());
    }

    private IEnumerator showGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    private IEnumerator bloodyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        var image = bloodyScreen.GetComponentInChildren<Image>();
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;


            elapsedTime += Time.deltaTime;

            yield return null; ; 
        }

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (isDead == false)
            {
               takeDamage(other.gameObject.GetComponent<ZombieHand>().damage);
            }
            
        }
    }
}
