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
        public event Action OnRouteCompleted;

        public string NextStopName => currentStopIndex < routeStops.Count ? routeStops[currentStopIndex].name : "Arusha reached";
        public bool IsRouteComplete => currentStopIndex >= routeStops.Count;

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

            RouteStop stop = routeStops[currentStopIndex];
            if (Vector3.Distance(bus.transform.position, stop.position) < 12f && bus.SpeedKmh < 5f && bus.doorsOpen)
            {
                var reachedStop = stop.name;
                OnStopReached?.Invoke(reachedStop);
                if (passengerSystem != null)
                {
                    passengerSystem.OnStopReached(reachedStop, currentStopIndex == routeStops.Count - 1);
                }

                currentStopIndex++;
                bus.ToggleDoorState();
                if (IsRouteComplete)
                {
                    OnRouteCompleted?.Invoke();
                }
            }
        }

        public void CreateDefaultRoute()
        {
            routeStops = new List<RouteStop>
            {
                new RouteStop { name = "Dar es Salaam", position = new Vector3(-4.5f, 0f, 0f) },
                new RouteStop { name = "Morogoro", position = new Vector3(-4.5f, 0f, 170f) },
                new RouteStop { name = "Dodoma", position = new Vector3(-4.5f, 0f, 360f) },
                new RouteStop { name = "Babati", position = new Vector3(-4.5f, 0f, 560f) },
                new RouteStop { name = "Arusha", position = new Vector3(-4.5f, 0f, 760f) }
            };
        }
    }
}
