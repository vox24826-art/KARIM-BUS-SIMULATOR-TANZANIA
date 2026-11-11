using System;
using System.Collections.Generic;
using UnityEngine;

namespace KarimBusSimulator
{
    [Serializable]
    public class RouteStop
    {
        public string name;
        public Vector3 position;
    }

    public class RouteSystem : MonoBehaviour
    {
        public List<RouteStop> routeStops = new List<RouteStop>();
        public BusController bus;
        public PassengerSystem passengerSystem;
        public int currentStopIndex = 0;

        public event Action<string> OnStopReached;

        private void Update()
        {
            if (bus == null || routeStops.Count == 0)
            {
                return;
            }

            if (currentStopIndex >= routeStops.Count)
            {
                return;
            }

            if (Vector3.Distance(bus.transform.position, routeStops[currentStopIndex].position) < 18f)
            {
                var reachedStop = routeStops[currentStopIndex].name;
                OnStopReached?.Invoke(reachedStop);
                if (passengerSystem != null)
                {
                    passengerSystem.OnStopReached(reachedStop);
                }

                currentStopIndex++;
            }
        }

        public void CreateDefaultRoute()
        {
            routeStops = new List<RouteStop>
            {
                new RouteStop { name = "Dar es Salaam", position = new Vector3(0f, 0f, 0f) },
                new RouteStop { name = "Morogoro", position = new Vector3(0f, 0f, 170f) },
                new RouteStop { name = "Dodoma", position = new Vector3(0f, 0f, 360f) },
                new RouteStop { name = "Manyara", position = new Vector3(0f, 0f, 560f) },
                new RouteStop { name = "Arusha", position = new Vector3(0f, 0f, 760f) }
            };
        }
    }
}
