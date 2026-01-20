using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false;

    // Komponenty efektu œmierci
    private SkinnedMeshRenderer skinnedMesh;
    private Animator animator;
    private PlantController pulsingScript;

    // Licznik
    private GameController gameController;

    void Start()
    {
        currentHealth = maxHealth;

        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponentInChildren<Animator>();
        pulsingScript = GetComponent<PlantController>();

        // fallback, jeœli spawner nie poda referencji
        if (gameController == null)
            gameController = FindObjectOfType<GameController>();
    }

    // Ustawiane ze spawnera (szybciej i czyœciej ni¿ Find)
    public void SetGameController(GameController gc)
    {
        gameController = gc;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (animator != null) animator.enabled = false;
        if (pulsingScript != null) pulsingScript.enabled = false;

        StartCoroutine(PerformDeathPuff());
    }

    IEnumerator PerformDeathPuff()
    {
        if (skinnedMesh != null && skinnedMesh.sharedMesh != null && skinnedMesh.sharedMesh.blendShapeCount > 0)
        {
            float duration = 0.5f;
            float elapsed = 0f;

            int puffIndex = 0;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float currentWeight = Mathf.Lerp(0f, 100f, elapsed / duration);
                skinnedMesh.SetBlendShapeWeight(puffIndex, currentWeight);
                yield return null;
            }

            skinnedMesh.SetBlendShapeWeight(puffIndex, 100f);
        }

        yield return new WaitForSeconds(0.2f);

        // ZG£OŒ DO LICZNIKA
        if (gameController != null)
            gameController.RegisterDestroyedPlant();

        Destroy(gameObject);
    }
}
