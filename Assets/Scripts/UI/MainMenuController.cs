using UnityEngine;
using UnityEngine.SceneManagement;

namespace KarimBusSimulator
{
    public class MainMenuController : MonoBehaviour
    {
        public void PlayGame()
        {
            SceneManager.LoadScene("MainGameScene");
        }

        public void OpenGarage()
        {
            Debug.Log("Garage opened");
        }

        public void OpenRoutes()
        {
            Debug.Log("Routes opened");
        }

        public void OpenCareer()
        {
            Debug.Log("Career opened");
        }

        public void OpenSettings()
        {
            Debug.Log("Settings opened");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
