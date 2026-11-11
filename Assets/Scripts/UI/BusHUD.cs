using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class BusHUD : MonoBehaviour
    {
        public BusController bus;
        public Text speedText;
        public Text rpmText;
        public Text gearText;
        public Text fuelText;
        public Text routeText;

        private void Update()
        {
            if (bus == null)
            {
                return;
            }

            if (speedText != null)
            {
                speedText.text = "Speed: " + Mathf.RoundToInt(bus.SpeedKmh) + " km/h";
            }

            if (rpmText != null)
            {
                rpmText.text = "RPM: " + Mathf.RoundToInt(bus.EngineRpm);
            }

            if (gearText != null)
            {
                gearText.text = bus.GearLabel;
            }

            if (fuelText != null)
            {
                fuelText.text = "Fuel: " + Mathf.RoundToInt((bus.Fuel / bus.maxFuel) * 100f) + "%";
            }

            if (routeText != null)
            {
                routeText.text = "Route: Dar es Salaam -> Arusha";
            }
        }
    }
}
