using System.Collections.Generic;
using UnityEngine;

namespace KarimBusSimulator
{
    public class RouteProgressionManager : MonoBehaviour
    {
        [System.Serializable]
        public class RouteDefinition
        {
            public string routeId;
            public string routeName;
            public string fromCity;
            public string toCity;
            public int unlockDriverLevel;
            public float distanceKm;
            public bool unlocked;
            public bool completed;
        }

        public List<RouteDefinition> routes = new List<RouteDefinition>();
        public string currentRouteId = "DAR_ES_SALAAM_TO_ARUSHA";

        private void Awake()
        {
            InitializeDefaults();
        }

        public void InitializeDefaults()
        {
            if (routes != null && routes.Count > 0)
            {
                return;
            }

            routes = new List<RouteDefinition>
            {
                new RouteDefinition { routeId = "DAR_ES_SALAAM_TO_ARUSHA", routeName = "Dar es Salaam -> Arusha", fromCity = "Dar es Salaam", toCity = "Arusha", unlockDriverLevel = 1, distanceKm = 640f, unlocked = true, completed = false },
                new RouteDefinition { routeId = "DAR_ES_SALAAM_TO_MOROGORO", routeName = "Dar es Salaam -> Morogoro", fromCity = "Dar es Salaam", toCity = "Morogoro", unlockDriverLevel = 1, distanceKm = 200f, unlocked = false, completed = false },
                new RouteDefinition { routeId = "MOROGORO_TO_DODOMA", routeName = "Morogoro -> Dodoma", fromCity = "Morogoro", toCity = "Dodoma", unlockDriverLevel = 2, distanceKm = 300f, unlocked = false, completed = false },
                new RouteDefinition { routeId = "DODOMA_TO_ARUSHA", routeName = "Dodoma -> Arusha", fromCity = "Dodoma", toCity = "Arusha", unlockDriverLevel = 3, distanceKm = 400f, unlocked = false, completed = false }
            };
        }

        public RouteDefinition GetCurrentRoute()
        {
            foreach (var route in routes)
            {
                if (route.routeId == currentRouteId)
                {
                    return route;
                }
            }

            return routes.Count > 0 ? routes[0] : null;
        }

        public void SetCurrentRoute(string routeId)
        {
            currentRouteId = routeId;
        }

        public void UnlockRoute(string routeId, int driverLevel)
        {
            foreach (var route in routes)
            {
                if (route.routeId == routeId)
                {
                    route.unlocked = driverLevel >= route.unlockDriverLevel;
                    if (route.unlocked)
                    {
                        currentRouteId = routeId;
                    }
                }
            }
        }

        public void CompleteCurrentRoute()
        {
            var current = GetCurrentRoute();
            if (current == null)
            {
                return;
            }

            current.completed = true;
            if (routes.IndexOf(current) < routes.Count - 1)
            {
                var next = routes[routes.IndexOf(current) + 1];
                next.unlocked = true;
            }
        }
    }
}
