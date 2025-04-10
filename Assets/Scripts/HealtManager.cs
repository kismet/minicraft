using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class HealthManager : MonoBehaviour
{
    [Header("UI Cuori")]
    public List<RawImage> heartImages;

    [Header("Settings")]
    public float fallDamageThreshold = 10f;
    public int maxHearts = 8;
    public string gameOverSceneName = "GameOver";

    private int currentHearts;
    private float lastYPosition;
    private bool isGrounded;

    void Start()
    {
        currentHearts = maxHearts;
        lastYPosition = transform.position.y;
        UpdateHeartsUI();
    }

    void Update()
    {
        CheckFallDamage();
    }

    void CheckFallDamage()
    {
        // Verifica quando il player cade e tocca il suolo
        float currentY = transform.position.y;

        if (!isGrounded && IsGrounded())
        {
            float fallDistance = lastYPosition - currentY;
            if (fallDistance > fallDamageThreshold)
            {
                int damage = Mathf.FloorToInt(fallDistance - fallDamageThreshold);
                TakeDamage(damage);
            }
        }

        if (IsGrounded())
            lastYPosition = currentY;

        isGrounded = IsGrounded();
    }

    // Metodo per calcolare se il player tocca terra
    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    public void TakeDamage(int amount)
    {
        currentHearts -= amount;
        currentHearts = Mathf.Clamp(currentHearts, 0, maxHearts);

        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            SceneManager.LoadScene(gameOverSceneName);
        }
    }




    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            heartImages[i].enabled = i < currentHearts;
        }
    }

    // Danno da contatto con cactus
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cactus"))
        {
            TakeDamage(1);
        }
    }
}
