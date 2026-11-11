using UnityEngine;

namespace KarimBusSimulator
{
    public class TrafficVehicleController : MonoBehaviour
    {
        public float speed = 10f;
        public float routeStart = 0f;
        public float routeEnd = 760f;
        public int direction = 1;

        private void Update()
        {
            Vector3 position = transform.position;
            position.z += speed * direction * Time.deltaTime;

            if (direction > 0 && position.z > routeEnd)
            {
                position.z = routeStart;
            }
            else if (direction < 0 && position.z < routeStart)
            {
                position.z = routeEnd;
            }

            transform.position = position;
        }
    }
}