using UnityEngine;

namespace KarimBusSimulator
{
    public class GameSettings : MonoBehaviour
    {
        public GraphicsTier graphicsTier = GraphicsTier.High;
        public int fpsLimit = 60;
        public float masterVolume = 1f;
        public float musicVolume = 0.8f;
        public float steeringSensitivity = 1f;
        public float cameraSensitivity = 1f;
        public string language = "English";

        public void ApplyDefaults()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fpsLimit;
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetInt("graphics_tier", (int)graphicsTier);
            PlayerPrefs.SetInt("fps_limit", fpsLimit);
            PlayerPrefs.SetFloat("master_volume", masterVolume);
            PlayerPrefs.SetFloat("music_volume", musicVolume);
            PlayerPrefs.SetFloat("steering_sensitivity", steeringSensitivity);
            PlayerPrefs.SetFloat("camera_sensitivity", cameraSensitivity);
            PlayerPrefs.SetString("language", language);
            PlayerPrefs.Save();
        }

        public void LoadSettings()
        {
            graphicsTier = (GraphicsTier)PlayerPrefs.GetInt("graphics_tier", (int)GraphicsTier.High);
            fpsLimit = PlayerPrefs.GetInt("fps_limit", 60);
            masterVolume = PlayerPrefs.GetFloat("master_volume", 1f);
            musicVolume = PlayerPrefs.GetFloat("music_volume", 0.8f);
            steeringSensitivity = PlayerPrefs.GetFloat("steering_sensitivity", 1f);
            cameraSensitivity = PlayerPrefs.GetFloat("camera_sensitivity", 1f);
            language = PlayerPrefs.GetString("language", "English");
        }
    }
}
