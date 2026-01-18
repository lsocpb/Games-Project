using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [Header("Ekrany")]
    public GameObject mainScreen;     // Menu G³ówne
    public GameObject loadScreen;     // Wczytywanie (Sloty)
    public GameObject settingsScreen; // Ustawienia

    // --- NAWIGACJA MENU G£ÓWNEGO ---

    public void OpenLoadGameScreen()
    {
        mainScreen.SetActive(false);
        loadScreen.SetActive(true);
        settingsScreen.SetActive(false); // Dla pewnoœci
    }

    public void OpenSettings()
    {
        mainScreen.SetActive(false);
        settingsScreen.SetActive(true);  // Poka¿ ustawienia
        loadScreen.SetActive(false);
    }

    // --- POWROTY ---

    public void BackToMainMenu()
    {
        // Ta funkcja zadzia³a dla obu ekranów (zamknie wszystko inne i poka¿e menu)
        loadScreen.SetActive(false);
        settingsScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    // --- SLOTY ---

    public void LoadSlot(int slotNumber)
    {
        Debug.Log("Symulacja: Wczytywanie gry ze slotu numer: " + slotNumber);
        SceneManager.LoadScene("Game");
    }

    // --- FUNKCJE GRY ---

    public void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Zamykanie gry...");
    }
}