using UnityEngine;

namespace KarimBusSimulator
{
    public class GameRuntime : MonoBehaviour
    {
        public BusController busController;
        public DriverCareer career;
        public RouteProgressionManager routeProgression;
        public GraphicsQualityManager graphicsQualityManager;
        public WeatherSystem weatherSystem;

        private SaveData saveData;

        private void Awake()
        {
            saveData = SaveSystem.Load();
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            EnsureSystems();
            LoadProgress();
        }

        private void EnsureSystems()
        {
            if (busController == null)
            {
                busController = FindAnyObjectByType<BusController>();
            }

            if (career == null)
            {
                career = FindAnyObjectByType<DriverCareer>();
            }

            if (routeProgression == null)
            {
                routeProgression = FindAnyObjectByType<RouteProgressionManager>();
            }

            if (routeProgression != null)
            {
                routeProgression.InitializeDefaults();
            }

            if (graphicsQualityManager == null)
            {
                graphicsQualityManager = FindAnyObjectByType<GraphicsQualityManager>();
            }

            if (graphicsQualityManager != null)
            {
                graphicsQualityManager.ApplyTier(GraphicsTier.High);
            }

            if (weatherSystem == null)
            {
                weatherSystem = FindAnyObjectByType<WeatherSystem>();
            }
        }

        private void LoadProgress()
        {
            if (saveData == null)
            {
                saveData = new SaveData();
            }

            if (career != null)
            {
                career.money = saveData.money;
                career.driverLevel = saveData.driverLevel;
                career.reputation = saveData.reputation;
                career.completedTrips = saveData.completedTrips;
            }

            if (busController != null && saveData.currentFuel > 0f)
            {
                // Fuel is stored but the bus controller uses internal fuel value.
                // This can be wired to a bus-specific save when the production bus prefab exists.
                busController.SetThrottle(0f);
            }

            if (routeProgression != null)
            {
                routeProgression.SetCurrentRoute(saveData.currentRoute);
            }
        }

        public void SaveProgress()
        {
            if (career != null)
            {
                saveData.money = career.money;
                saveData.driverLevel = career.driverLevel;
                saveData.reputation = career.reputation;
                saveData.completedTrips = career.completedTrips;
                saveData.totalDistance = career.totalDistanceKm;
            }

            if (routeProgression != null)
            {
                saveData.currentRoute = routeProgression.currentRouteId;
            }

            SaveSystem.Save(saveData);
        }
    }
}
