using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    // PAUZA
    [Header("Pause UI")]
    public GameObject pausePanel;
    private bool isPaused = false;

    // LICZNIK ROSLIN (UI)
    [Header("Plants Counter UI")]
    public TMP_Text plantsCounterText;

    private int totalPlants = 0;
    private int destroyedPlants = 0;

    void Start()
    {
        // Przy generowaniu runtime liczymy total przez spawner
        totalPlants = 0;
        destroyedPlants = 0;
        UpdatePlantsUI();
    }

    void Update()
    {
        // ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // ---- PAUZA ----
    public void PauseGame()
    {
        if (pausePanel != null) pausePanel.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void SaveGame()
    {
        Debug.Log("ZAPISYWANIE GRY... [Symulacja slot w]");
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    // ---- RO LINY: API dla spawnera i EnemyHealth ----
    public void RegisterSpawnedPlant()
    {
        totalPlants++;
        UpdatePlantsUI();
    }

    public void RegisterDestroyedPlant()
    {
        destroyedPlants++;
        UpdatePlantsUI();
    }

    private void UpdatePlantsUI()
    {
        int left = Mathf.Max(0, totalPlants - destroyedPlants);

        if (plantsCounterText != null)
        {
            plantsCounterText.text =
                $"Plants: \n{destroyedPlants} destroyed \n{left} left \n{totalPlants} total";
        }
    }
}
