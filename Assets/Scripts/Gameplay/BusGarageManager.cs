using System.Collections.Generic;
using UnityEngine;

namespace KarimBusSimulator
{
    public class BusGarageManager : MonoBehaviour
    {
        public BusCatalog busCatalog;
        public List<BusDefinition> ownedBuses = new List<BusDefinition>();
        public BusDefinition selectedBus;

        private void Start()
        {
            if (busCatalog == null)
            {
                busCatalog = FindAnyObjectByType<BusCatalog>();
            }

            if (busCatalog != null)
            {
                busCatalog.BuildDefaultCatalog();
                ownedBuses = new List<BusDefinition>();
                foreach (var bus in busCatalog.buses)
                {
                    if (bus.owned)
                    {
                        ownedBuses.Add(bus);
                    }
                }

                if (ownedBuses.Count > 0)
                {
                    selectedBus = ownedBuses[0];
                }
            }
        }

        public bool BuyBus(string busId, int currentMoney)
        {
            if (busCatalog == null)
            {
                return false;
            }

            var bus = busCatalog.GetBusById(busId);
            if (bus == null || bus.owned)
            {
                return false;
            }

            if (currentMoney < bus.price)
            {
                return false;
            }

            bus.owned = true;
            ownedBuses.Add(bus);
            selectedBus = bus;
            return true;
        }

        public void SelectBus(string busId)
        {
            if (busCatalog == null)
            {
                return;
            }

            selectedBus = busCatalog.GetBusById(busId);
        }

        public void RepairSelectedBus()
        {
            if (selectedBus == null)
            {
                return;
            }

            Debug.Log("Repairing bus: " + selectedBus.busName);
        }
    }
}
