using UnityEngine;

namespace KarimBusSimulator
{
    public class AndroidBuildChecklist : MonoBehaviour
    {
        public string packageName = "com.karimgames.karimbussimulatortanzania";
        public string productName = "KARIM BUS SIMULATOR TANZANIA";
        public string companyName = "KARIM GAMES STUDIO LIMITED";
        public bool useIL2CPP = true;
        public bool installOnAndroid = true;
        public bool allowDebugging = true;
        public int minSdkVersion = 24;
        public int targetSdkVersion = 34;

        public void ValidateConfiguration()
        {
            Debug.Log("Android build checklist ready");
            Debug.Log("Package: " + packageName);
            Debug.Log("Product: " + productName);
            Debug.Log("Company: " + companyName);
            Debug.Log("IL2CPP: " + useIL2CPP);
            Debug.Log("Min SDK: " + minSdkVersion + " | Target SDK: " + targetSdkVersion);
        }
    }
}
