using UnityEngine;

namespace KarimBusSimulator
{
    public class PlayerProgressTracker : MonoBehaviour
    {
        public DriverCareer career;
        public RouteProgressionManager routeProgression;
        public MissionManager missionManager;

        private void Start()
        {
            if (career == null)
            {
                career = FindAnyObjectByType<DriverCareer>();
            }

            if (routeProgression == null)
            {
                routeProgression = FindAnyObjectByType<RouteProgressionManager>();
            }

            if (missionManager == null)
            {
                missionManager = FindAnyObjectByType<MissionManager>();
            }
        }

        public void UpdateProgress()
        {
            if (career == null)
            {
                return;
            }

            if (routeProgression != null)
            {
                Debug.Log("Current route: " + routeProgression.currentRouteId);
            }

            if (missionManager != null)
            {
                Debug.Log("Mission count: " + missionManager.missions.Count);
            }

            Debug.Log("Career money: " + career.money + " | trips: " + career.completedTrips + " | level: " + career.driverLevel);
        }
    }
}
