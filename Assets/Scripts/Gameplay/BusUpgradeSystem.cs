using UnityEngine;

namespace KarimBusSimulator
{
    public class BusUpgradeSystem : MonoBehaviour
    {
        public int engineLevel = 1;
        public int suspensionLevel = 1;
        public int tireLevel = 1;
        public int brakeLevel = 1;

        public void UpgradeEngine()
        {
            engineLevel += 1;
            Debug.Log("Engine upgraded to level " + engineLevel);
        }

        public void UpgradeSuspension()
        {
            suspensionLevel += 1;
            Debug.Log("Suspension upgraded to level " + suspensionLevel);
        }

        public void UpgradeTires()
        {
            tireLevel += 1;
            Debug.Log("Tires upgraded to level " + tireLevel);
        }

        public void UpgradeBrakes()
        {
            brakeLevel += 1;
            Debug.Log("Brakes upgraded to level " + brakeLevel);
        }
    }
}
