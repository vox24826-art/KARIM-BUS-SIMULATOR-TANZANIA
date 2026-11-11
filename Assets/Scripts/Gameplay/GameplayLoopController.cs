using UnityEngine;

namespace KarimBusSimulator
{
    public class GameplayLoopController : MonoBehaviour
    {
        public RouteGameplayController routeGameplayController;
        public RouteProgressionManager progression;
        public DriverCareer career;
        public MissionManager missions;

        private void Start()
        {
            if (routeGameplayController == null)
            {
                routeGameplayController = FindAnyObjectByType<RouteGameplayController>();
            }

            if (progression == null)
            {
                progression = FindAnyObjectByType<RouteProgressionManager>();
            }

            if (career == null)
            {
                career = FindAnyObjectByType<DriverCareer>();
            }

            if (missions == null)
            {
                missions = FindAnyObjectByType<MissionManager>();
            }

            if (progression != null)
            {
                progression.InitializeDefaults();
            }

            if (routeGameplayController != null)
            {
                routeGameplayController.BeginRoute("DAR_ES_SALAAM_TO_ARUSHA");
            }
        }

        public void TriggerRouteComplete()
        {
            if (routeGameplayController != null)
            {
                routeGameplayController.FinishRoute(true, 91f);
            }
        }
    }
}
