using UnityEngine;

namespace KarimBusSimulator
{
    public class DriverCareer : MonoBehaviour
    {
        public int money = 2500;
        public int reputation = 0;
        public int driverLevel = 1;
        public int completedTrips = 0;
        public float totalDistanceKm = 0f;
        public float passengerSatisfaction = 84f;
        public float safetyRating = 80f;

        public void RegisterTrip(float distanceKm, int passengers, float satisfaction, bool safe)
        {
            totalDistanceKm += distanceKm;
            completedTrips += 1;
            passengerSatisfaction = Mathf.Clamp(satisfaction, 0f, 100f);
            safetyRating = safe ? Mathf.Clamp(safetyRating + 2f, 0f, 100f) : Mathf.Clamp(safetyRating - 5f, 0f, 100f);
            reputation = Mathf.Max(0, reputation + Mathf.RoundToInt(passengers * 0.2f));
            money += Mathf.RoundToInt(distanceKm * 12f + passengers * 3f);

            if (completedTrips >= 3)
            {
                driverLevel = 2;
            }
            if (completedTrips >= 8)
            {
                driverLevel = 3;
            }
        }
    }
}
