using UnityEngine;

namespace KarimBusSimulator
{
    public class PassengerSystem : MonoBehaviour
    {
        public int waitingPassengers = 24;
        public int boardedPassengers = 0;
        public int totalTrips = 0;
        public float satisfaction = 84f;
        public int busCapacity = 48;
        public int passengersPerStop = 6;

        public void OnStopReached(string stopName)
        {
            OnStopReached(stopName, false);
        }

        public void OnStopReached(string stopName, bool destination)
        {
            if (string.IsNullOrEmpty(stopName))
            {
                return;
            }

            if (destination)
            {
                boardedPassengers = 0;
            }
            else if (waitingPassengers > 0)
            {
                int boarding = Mathf.Min(passengersPerStop, waitingPassengers, busCapacity - boardedPassengers);
                boardedPassengers += boarding;
                waitingPassengers -= boarding;
            }

            totalTrips++;
            satisfaction = Mathf.Clamp(satisfaction + 0.8f, 0f, 100f);
        }

        public void ApplyDrivingFeedback(float safeDriving, float smoothness)
        {
            var combined = Mathf.Clamp((safeDriving + smoothness) * 0.5f, 0f, 100f);
            satisfaction = Mathf.Clamp(combined, 0f, 100f);
        }
    }
}
