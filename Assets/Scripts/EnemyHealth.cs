using UnityEngine;
using System.Collections; // Potrzebne do korutyn (IEnumerator)

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    private int currentHealth;
    private bool isDead = false; // ¯eby nie umieraæ dwa razy

    // Komponenty potrzebne do efektu œmierci
    private SkinnedMeshRenderer skinnedMesh;
    private Animator animator;
    private PlantController pulsingScript;

    void Start()
    {
        currentHealth = maxHealth;

        // Szukamy komponentów w roœlinie
        skinnedMesh = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponentInChildren<Animator>();
        pulsingScript = GetComponent<PlantController>();
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return; // Jeœli ju¿ nie ¿yje, ignoruj strza³y

        currentHealth -= damage;
        // Opcjonalnie: Zmieñ kolor na chwilê przy trafieniu (flash)
        Debug.Log(gameObject.name + " HP: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // 1. Wy³¹czamy inne skrypty/animatory, ¿eby nie walczy³y z efektem œmierci
        if (animator != null) animator.enabled = false;
        if (pulsingScript != null) pulsingScript.enabled = false;

        // 2. Uruchamiamy sekwencjê œmierci rozci¹gniêt¹ w czasie
        StartCoroutine(PerformDeathPuff());
    }

    // --- TO JEST TA MAGIA (KORUTYNA) ---
    IEnumerator PerformDeathPuff()
    {
        // Sprawdzamy, czy mamy czym ruszaæ
        if (skinnedMesh != null && skinnedMesh.sharedMesh.blendShapeCount > 0)
        {
            float duration = 0.5f; // Czas trwania "puchniêcia" (pó³ sekundy)
            float elapsed = 0f;

            // Zak³adamy, ¿e "puff" to pierwszy BlendShape (indeks 0). 
            // Jeœli masz ich wiêcej, upewnij siê, ¿e to ten w³aœciwy.
            int puffIndex = 0;

            // Pêtla, która wykonuje siê przez 'duration' czasu
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                // Lerp p³ynnie zmienia wartoœæ od 0 do 100 w czasie
                float currentWeight = Mathf.Lerp(0f, 100f, elapsed / duration);
                skinnedMesh.SetBlendShapeWeight(puffIndex, currentWeight);

                // Czekamy do nastêpnej klatki obrazu
                yield return null;
            }
            // Na koniec upewniamy siê, ¿e jest na 100%
            skinnedMesh.SetBlendShapeWeight(puffIndex, 100f);
        }

        // 3. Czekamy chwilê, ¿eby nacieszyæ oko spuchniêt¹ roœlin¹
        yield return new WaitForSeconds(0.2f);

        // 4. ¯egnamy roœlinê
        Destroy(gameObject);
    }
}