using UnityEngine;
using UnityEngine.SceneManagement; // Potrzebne do zmiany scen

public class GameController : MonoBehaviour
{
    // Tutaj przypiszemy nasz panel pauzy w Unity
    public GameObject pausePanel;

    // Zmienna, która pamiêta, czy gra jest spauzowana
    private bool isPaused = false;

    void Update()
    {
        // Sprawdzamy, czy wciœniêto ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame(); // Jak jest pauza -> wznów
            }
            else
            {
                PauseGame(); // Jak gra -> pauzuj
            }
        }
    }

    public void PauseGame()
    {
        pausePanel.SetActive(true); // Poka¿ panel
        Time.timeScale = 0f; // ZATRZYMAJ CZAS w grze (fizyka staje)
        isPaused = true;
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false); // Ukryj panel
        Time.timeScale = 1f; // Wznów czas
        isPaused = false;
    }

    public void SaveGame()
    {
        // Symulacja zapisu (zgodnie z wymaganiami)
        Debug.Log("ZAPISYWANIE GRY... [Symulacja slotów]");
        // Tu w przysz³oœci by³by kod zapisuj¹cy pozycjê gracza
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f; // Wa¿ne! Zanim wyjdziemy, wznów czas, bo zostanie zatrzymany w menu!
        SceneManager.LoadScene("Menu"); // Wróæ do sceny 0
    }
}