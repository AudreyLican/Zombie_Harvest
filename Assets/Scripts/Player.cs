using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        if (playerHealthUI != null)
        {
            playerHealthUI.text = $"Health: {HP}";
        }
        else
        {
            Debug.LogError("PlayerHealthUI is not assigned.");
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return; // If already dead, do nothing

        HP -= damageAmount;

        if (HP <= 0)
        {
            Debug.Log("Player Dead");
            PlayerDead();
            isDead = true;
        }
        else
        {
            Debug.Log("Player Hit");
            StartCoroutine(BloddyScreenEffect());
            if (playerHealthUI != null)
            {
                playerHealthUI.text = $"Health: {HP}";
            }
            else
            {
                Debug.LogError("PlayerHealthUI is not assigned.");
            }
            if (SoundManager.Instance != null && SoundManager.Instance.playerChannel != null)
            {
                SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerHurt);
            }
            else
            {
                Debug.LogError("SoundManager or playerChannel is not assigned.");
            }
        }
    }

    public void RestoreHealth(int healAmount)
    {
        HP += healAmount;

        if (HP > maxHP)
        {
            HP = maxHP;
        }

        if (playerHealthUI != null)
        {
            playerHealthUI.text = $"Health: {HP}";
        }
        else
        {
            Debug.LogError("PlayerHealthUI is not assigned.");
        }
    }

    private void PlayerDead()
    {
        if (SoundManager.Instance != null && SoundManager.Instance.playerChannel != null)
        {
            SoundManager.Instance.playerChannel.PlayOneShot(SoundManager.Instance.playerDeath);
            SoundManager.Instance.playerChannel.clip = SoundManager.Instance.gameOverMusic; // to get delay, before playing the music
            SoundManager.Instance.playerChannel.PlayDelayed(2f);
        }
        else
        {
            Debug.LogError("SoundManager or playerChannel is not assigned.");
        }

        var mouseMovement = GetComponent<MouseMouvement>();
        if (mouseMovement != null) mouseMovement.enabled = false;
        var playerMovement = GetComponent<PlayerMouvement>();
        if (playerMovement != null) playerMovement.enabled = false;

        // Dying Animation
        var animator = GetComponentInChildren<Animator>();
        if (animator != null) animator.enabled = true;
        if (playerHealthUI != null)
        {
            playerHealthUI.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogError("PlayerHealthUI is not assigned.");
        }

        var screenFader = GetComponent<ScreenFader>();
        if (screenFader != null)
        {
            screenFader.StartFade();
        }
        else
        {
            Debug.LogError("ScreenFader is not assigned.");
        }

        StartCoroutine(ShowGameOverUI());
    }

    private IEnumerator ShowGameOverUI()
    {
        yield return new WaitForSeconds(1f);

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
        else
        {
            Debug.LogError("GameOverUI is not assigned.");
        }

        // Save the last player survived
        if (GlobalReferences.Instance != null)
        {
            int wavesurvived = GlobalReferences.Instance.waveNumber;
            if (SaveLoadManager.Instance != null)
            {
                if (wavesurvived - 1 > SaveLoadManager.Instance.LoadHighScore())
                {
                    SaveLoadManager.Instance.SaveHighScore(wavesurvived - 1); // player dead current wave so last wave survived is previous
                }
            }
            else
            {
                Debug.LogError("SaveLoadManager is not assigned.");
            }
        }
        else
        {
            Debug.LogError("GlobalReferences is not assigned.");
        }

        StartCoroutine(ReturnToMainMenu());
    }

    private IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(4f);

        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene("MainMenu");
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
                    var zombieAttackHand = other.gameObject.GetComponent<ZombieAttackHand>();
                    if (zombieAttackHand != null)
                    {
                        TakeDamage(zombieAttackHand.damage);
                        StartCoroutine(BloddyScreenEffect());
                    }
                    else
                    {
                        Debug.LogError("ZombieAttackHand component is not found on the zombie hand.");
                    }
                }
            }
        }
    }

    private IEnumerator BloddyScreenEffect()
    {
        if (bloodyScreen != null)
        {
            if (bloodyScreen.activeInHierarchy == false)
            {
                bloodyScreen.SetActive(true);
            }

            // --- Fade Effect --- //
            var image = bloodyScreen.GetComponentInChildren<Image>();
            if (image != null)
            {
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

                    yield return null; // Wait for the next frame.
                }
            }
            else
            {
                Debug.LogError("Image component is not found in bloodyScreen.");
            }
            // --- end Fade effect --- //

            if (bloodyScreen.activeInHierarchy)
            {
                bloodyScreen.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("BloodyScreen is not assigned.");
        }
    }
}
