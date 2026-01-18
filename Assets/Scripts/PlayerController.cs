using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public float moveSpeed = 4.0f;
    public float rotationSpeed = 100.0f;

    // Miejsce na dźwięk (Audio Source)
    private AudioSource audioSource;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        // Pobieramy AudioSource, który przed chwilą dodałeś
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // --- 1. RUCH (To co było) ---
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * v * moveSpeed * Time.deltaTime);
        transform.Rotate(Vector3.up * h * rotationSpeed * Time.deltaTime);

        animator.SetFloat("Speed", Mathf.Abs(v));

        // --- 2. STRZELANIE (Nowość) ---
        // "Fire1" to domyślnie Lewy Przycisk Myszy lub lewy Ctrl
        if (Input.GetButtonDown("Fire1"))
        {
            // Odpalamy animację
            animator.SetTrigger("Shoot");

            // Odpalamy dźwięk (jeśli jakiś jest przypisany)
            if (audioSource != null && audioSource.clip != null)
            {
                // PlayOneShot jest lepsze do strzałów, bo pozwala 
                // nałożyć na siebie kilka dźwięków, jak klikasz szybko
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }
}