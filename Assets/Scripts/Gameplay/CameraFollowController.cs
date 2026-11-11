using UnityEngine;
using UnityEngine.EventSystems;

namespace KarimBusSimulator
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollowController : MonoBehaviour
    {
        public Transform target;
        public float followSharpness = 8f;
        public float orbitSensitivity = 0.18f;

        private int viewIndex;
        private float orbitYaw;

        private void LateUpdate()
        {
            if (target == null)
            {
                return;
            }

            ReadOrbitInput();
            Vector3 lookAt = target.position + Vector3.up * 1.8f;
            Vector3 desiredPosition;
            Quaternion desiredRotation;

            if (viewIndex == 1)
            {
                desiredPosition = target.TransformPoint(new Vector3(0f, 2.45f, 5.2f));
                desiredRotation = target.rotation;
            }
            else
            {
                Vector3 offset = Quaternion.Euler(0f, orbitYaw, 0f) * new Vector3(0f, 5f, -12f);
                desiredPosition = lookAt + target.rotation * offset;
                desiredRotation = Quaternion.LookRotation(lookAt - desiredPosition, Vector3.up);
            }

            float blend = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desiredPosition, blend);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, blend);
        }

        public void CycleView()
        {
            viewIndex = (viewIndex + 1) % 2;
        }

        private void ReadOrbitInput()
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.position.y < Screen.height * 0.4f || touch.position.x < Screen.width * 0.35f)
                {
                    return;
                }

                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                {
                    return;
                }

                if (touch.phase == TouchPhase.Moved)
                {
                    orbitYaw += touch.deltaPosition.x * orbitSensitivity;
                }
            }
            else if (Input.GetMouseButton(0) && Input.mousePosition.y > Screen.height * 0.4f && Input.mousePosition.x > Screen.width * 0.35f)
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                orbitYaw += Input.GetAxis("Mouse X") * orbitSensitivity * 12f;
            }
        }
    }
}