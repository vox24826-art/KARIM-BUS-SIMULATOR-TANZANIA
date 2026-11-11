using System;
using System.IO;
using UnityEngine;

namespace KarimBusSimulator
{
    [Serializable]
    public class SaveData
    {
        public int money = 2500;
        public int driverLevel = 1;
        public int reputation = 0;
        public int completedTrips = 0;
        public float totalDistance = 0f;
        public string currentRoute = "DAR_ES_SALAAM_TO_ARUSHA";
        public float currentFuel = 100f;
        public int busesUnlocked = 1;
        public bool firstRouteCompleted;
        public string lastUpdated = DateTime.UtcNow.ToString("o");
    }

    public static class SaveSystem
    {
        private const string FileName = "karim_bus_save.json";

        public static SaveData Load()
        {
            var path = Path.Combine(Application.persistentDataPath, FileName);
            if (!File.Exists(path))
            {
                return new SaveData();
            }

            try
            {
                var json = File.ReadAllText(path);
                var data = JsonUtility.FromJson<SaveData>(json);
                return data ?? new SaveData();
            }
            catch (Exception)
            {
                return new SaveData();
            }
        }

        public static void Save(SaveData data)
        {
            var path = Path.Combine(Application.persistentDataPath, FileName);
            data.lastUpdated = DateTime.UtcNow.ToString("o");
            var json = JsonUtility.ToJson(data, true);
            File.WriteAllText(path, json);
        }
    }
}
