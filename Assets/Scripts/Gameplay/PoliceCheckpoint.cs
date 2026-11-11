using UnityEngine;

namespace KarimBusSimulator
{
    public class PoliceCheckpoint : MonoBehaviour
    {
        public string checkpointName = "Tanzania Highway Checkpoint";
        public float inspectionRadius = 20f;
        public bool checkpointActive;
        public bool busStopped;

        public void UpdateCheckpoint(BusController bus)
        {
            if (bus == null)
            {
                return;
            }

            checkpointActive = Vector3.Distance(transform.position, bus.transform.position) <= inspectionRadius;
            if (checkpointActive)
            {
                bus.SetBrake(1f);
                bus.SetHandbrake(true);
                busStopped = true;
                Debug.Log("Police checkpoint active: " + checkpointName + ". Driver must stop.");
            }
            else if (busStopped)
            {
                bus.SetHandbrake(false);
                busStopped = false;
            }
        }

        private void Update()
        {
            var bus = FindAnyObjectByType<BusController>();
            UpdateCheckpoint(bus);
        }
    }
}
