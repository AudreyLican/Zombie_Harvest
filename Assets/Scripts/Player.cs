using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public int HP = 100;
    public int maxHP = 100;
    public GameObject bloodyScreen;

    public TextMeshProUGUI playerHealthUI;
    public GameObject gameOverUI;

    public bool isDead;

    private void Start()
    {
        playerHealthUI.text = $"Health: {HP}";
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // If already dead, do nothing

        HP -= damageAmount;

        if (HP <= 0)
        {
            print("Player Dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            print("Player Hit");
            StartCoroutine(BloddyScreenEffect());
            playerHealthUI.text = $"Health: {HP}";
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
        }
    }

    public void RestoreHealth(int healAmount)
    {
        HP += healAmount;
        
        if (HP > maxHP)
        {
            HP = maxHP;
        }
        
        playerHealthUI.text = $"Health: {HP}";
    }

    private void PlayerDead()
    {
        SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);

        SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic; // to get delay, before playing the music
        SoundManager.Instance.playerChannel.PlayDelayed(2f);

        GetComponent<MouseMouvement>().enabled = false;
        GetComponent<PlayerMouvement>().enabled = false;

        // Dying Animation
        GetComponentInChildren<Animator>().enabled = true;
        playerHealthUI.gameObject.SetActive(false);

        GetComponent<ScreenFader>().StartFade();
        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);
        gameOverUI.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ZombieHand"))
        {
            if (!isDead)
            {
                var zombie = other.transform.root.GetComponent<Enemy>();
                if (zombie != null && !zombie.isDead && !isDead)
                {
                    TakeDamage(other.gameObject.GetComponent<ZombieAttackHand>().damage);
                    StartCoroutine(BloddyScreenEffect()); 
                }
            }
        }
    }

    private IEnumerator BloddyScreenEffect()
    {
        if (bloodyScreen.activeInHierarchy == false)
        {
            bloodyScreen.SetActive(true);
        }

        // --- Fade Effect --- //

        var image = bloodyScreen.GetComponentInChildren<Image>();

        // Set the initial alpha value to 1 (fully visible).
        Color startColor = image.color;
        startColor.a = 1f;
        image.color = startColor;

        float duration = 2f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value using Lerp.
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);

            // Update the color with the new alpha value.
            Color newColor = image.color;
            newColor.a = alpha;
            image.color = newColor;

            // Increment the elapsed time.
            elapsedTime += Time.deltaTime;

            yield return null; ; // Wait for the next frame.
        }
        // --- end Fade effect --- //

        if (bloodyScreen.activeInHierarchy)
        {
            bloodyScreen.SetActive(false);
        }
    }
}
