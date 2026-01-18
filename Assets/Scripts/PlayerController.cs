using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public float rotationSpeed = 100.0f;

    public Transform gunMuzzle;
    public GameObject hitEffect; // PAMIĘTAJ: Przypisz tu prefab iskier w Inspektorze!

    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // RUCH
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * h * rotationSpeed * Time.deltaTime);
        animator.SetFloat("Speed", Mathf.Abs(v));

        // STRZELANIE
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        animator.SetTrigger("Shoot");
        if (audioSource != null && audioSource.clip != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }

        RaycastHit hit;
        if (Physics.Raycast(gunMuzzle.position, gunMuzzle.forward, out hit, 100f))
        {
            // 1. Pokaż efekt trafienia (iskry) w miejscu uderzenia
            if (hitEffect != null)
            {
                Instantiate(hitEffect, hit.point, Quaternion.LookRotation(hit.normal));
            }

            // 2. Sprawdź, czy trafiliśmy we wroga z systemem zdrowia
            // Pobieramy skrypt EnemyHealth z obiektu, w który trafiliśmy
            EnemyHealth enemy = hit.transform.GetComponent<EnemyHealth>();

            if (enemy != null)
            {
                // Zadaj 1 punkt obrażeń
                enemy.TakeDamage(1);
            }
        }
    }
}