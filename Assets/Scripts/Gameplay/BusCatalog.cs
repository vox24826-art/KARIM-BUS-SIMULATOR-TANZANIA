using System.Collections.Generic;
using UnityEngine;

namespace KarimBusSimulator
{
    [System.Serializable]
    public class BusDefinition
    {
        public string id;
        public string busName;
        public string brand;
        public string variant;
        public int seats;
        public int unlockDriverLevel;
        public int price;
        public float maxSpeedKmh;
        public bool owned;
        public string livery;
        public string description;
    }

    public class BusCatalog : MonoBehaviour
    {
        public List<BusDefinition> buses = new List<BusDefinition>();

        private void Awake()
        {
            BuildDefaultCatalog();
        }

        public void BuildDefaultCatalog()
        {
            if (buses != null && buses.Count > 0)
            {
                return;
            }

            buses = new List<BusDefinition>
            {
                new BusDefinition { id = "scania_coach", busName = "Scania Coach", brand = "Scania", variant = "Intercity", seats = 48, unlockDriverLevel = 1, price = 0, owned = true, livery = "KARIM CLASSIC", description = "First playable intercity coach for Tanzania routes." },
                new BusDefinition { id = "mercedes_911", busName = "Mercedes Tourliner", brand = "Mercedes-Benz", variant = "Long Distance", seats = 52, unlockDriverLevel = 2, price = 38000, owned = false, livery = "Silverline", description = "A reliable long-haul coach for East African corridors." },
                new BusDefinition { id = "marcopolo_jet", busName = "Marcopolo Jet", brand = "Marcopolo", variant = "Executive Coach", seats = 46, unlockDriverLevel = 3, price = 52000, owned = false, livery = "Safari Gold", description = "Luxury coach for premium passengers and long corridors." },
                new BusDefinition { id = "yutong_express", busName = "Yutong Express", brand = "Yutong", variant = "City Link", seats = 39, unlockDriverLevel = 2, price = 34000, owned = false, livery = "Blue Route", description = "Flexible coach for urban and regional links." },
                new BusDefinition { id = "zhongtong_cx", busName = "Zhongtong CX", brand = "Zhongtong", variant = "Coach", seats = 44, unlockDriverLevel = 3, price = 46000, owned = false, livery = "Lake Safari", description = "Fit for long routes and varied terrain." }
            };
        }

        public BusDefinition GetBusById(string busId)
        {
            foreach (var bus in buses)
            {
                if (bus.id == busId)
                {
                    return bus;
                }
            }

            return buses.Count > 0 ? buses[0] : null;
        }
    }
}
