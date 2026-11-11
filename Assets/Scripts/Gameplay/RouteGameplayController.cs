using System.Collections.Generic;
using UnityEngine;

namespace KarimBusSimulator
{
    public class RouteGameplayController : MonoBehaviour
    {
        public RouteProgressionManager progression;
        public BusController busController;
        public DriverCareer driverCareer;
        public MissionManager missionManager;
        public float currentDistance;
        public float tripReward;

        private void Start()
        {
            if (progression == null)
            {
                progression = FindAnyObjectByType<RouteProgressionManager>();
            }

            if (busController == null)
            {
                busController = FindAnyObjectByType<BusController>();
            }

            if (driverCareer == null)
            {
                driverCareer = FindAnyObjectByType<DriverCareer>();
            }

            if (missionManager == null)
            {
                missionManager = FindAnyObjectByType<MissionManager>();
            }
        }

        public void BeginRoute(string routeId)
        {
            progression?.SetCurrentRoute(routeId);
            var route = progression?.GetCurrentRoute();
            if (route == null)
            {
                return;
            }

            currentDistance = route.distanceKm;
            tripReward = route.distanceKm * 12f;
            Debug.Log("Route started: " + route.routeName + " distance=" + currentDistance + " km");
        }

        public void FinishRoute(bool safe, float passengerSatisfaction)
        {
            if (driverCareer == null)
            {
                return;
            }

            var baseDistance = currentDistance > 0f ? currentDistance : 100f;
            driverCareer.RegisterTrip(baseDistance, 24, passengerSatisfaction, safe);

            if (progression != null)
            {
                progression.CompleteCurrentRoute();
            }

            if (missionManager != null)
            {
                missionManager.CompleteMission("first_route");
                missionManager.CompleteMission("checkpoint_pass");
            }

            Debug.Log("Route completed. Earnings: " + driverCareer.money);
        }
    }
}
