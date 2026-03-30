using UnityEngine;
using System.Collections;

public class TreeHealth : MonoBehaviour
{

    public int maxHealth = 5;
    private int currentHealth;
    public int CurrentHealth => currentHealth;
    public int MaxHealth => maxHealth;

    public GameObject chopHolder;
    public GameObject treeHealthUI;
    public AudioSource audioSource;
    public AudioClip chopSound;
    public AudioClip fallSound;

    public Transform treeTop;
    public bool isFalling = false;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isFalling) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (chopSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(chopSound);
        }

        Debug.Log("Tree Health: " + currentHealth);

        if (currentHealth <= 0)
        {
            StartCoroutine(FallTree());
        }
    }

    IEnumerator FallTree()
    {
        isFalling = true;

        if (fallSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(fallSound);
        }

        Quaternion startRotation = treeTop.rotation;
        Quaternion endRotation = Quaternion.Euler(treeTop.eulerAngles.x, treeTop.eulerAngles.y, 90f);

        float duration = 1.5f;
        float time = 0f;

        while (time < duration)
        {
            treeTop.rotation = Quaternion.Slerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        treeTop.rotation = endRotation;
    }
}