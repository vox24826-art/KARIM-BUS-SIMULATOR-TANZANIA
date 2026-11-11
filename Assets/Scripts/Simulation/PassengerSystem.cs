using UnityEngine;

namespace KarimBusSimulator
{
    public class PassengerSystem : MonoBehaviour
    {
        public int waitingPassengers = 24;
        public int boardedPassengers = 0;
        public int totalTrips = 0;
        public float satisfaction = 84f;

        public void OnStopReached(string stopName)
        {
            if (string.IsNullOrEmpty(stopName))
            {
                return;
            }

            if (waitingPassengers > 0)
            {
                boardedPassengers = Mathf.Min(boardedPassengers + 1, waitingPassengers);
                waitingPassengers = Mathf.Max(0, waitingPassengers - 1);
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
