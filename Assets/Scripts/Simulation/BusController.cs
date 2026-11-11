using UnityEngine;

namespace KarimBusSimulator
{
    [RequireComponent(typeof(Rigidbody))]
    public class BusController : MonoBehaviour
    {
        public enum TransmissionMode
        {
            Neutral,
            Drive,
            Reverse
        }

        [Header("Vehicle Tuning")]
        public float maxForwardSpeed = 120f;
        public float reverseSpeed = 25f;
        public float acceleration = 24f;
        public float brakeForce = 38f;
        public float steeringAngle = 32f;
        public float steeringSensitivity = 1f;
        public float maxFuel = 120f;
        public float fuelConsumption = 0.2f;
        public float massKg = 7000f;

        [Header("Status")]
        public bool isHandbrakeOn;
        public bool doorsOpen;
        public bool luggageOpen;
        public bool leftIndicatorOn;
        public bool rightIndicatorOn;
        public bool hazardsOn;
        public bool headlightsOn = true;
        public bool highBeamOn;
        public bool hornActive;

        [SerializeField] private float steeringInput;
        [SerializeField] private float throttleInput;
        [SerializeField] private float brakeInput;
        [SerializeField] private float fuel;
        [SerializeField] private float engineRpm;
        [SerializeField] private TransmissionMode transmission = TransmissionMode.Drive;

        private Rigidbody rb;

        public float SpeedKmh => Mathf.Abs(Vector3.Dot(rb.velocity, transform.forward)) * 3.6f;
        public float Fuel => fuel;
        public float EngineRpm => engineRpm;
        public string GearLabel => transmission == TransmissionMode.Reverse ? "R" : transmission == TransmissionMode.Neutral ? "N" : "D";
        public bool IsReverse => transmission == TransmissionMode.Reverse;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.mass = massKg;
            rb.drag = 0.08f;
            rb.angularDrag = 1.2f;
            fuel = maxFuel;
        }

        private void Update()
        {
            ReadKeyboardFallback();
            UpdateEngineRpm();

            if (fuel <= 0f)
            {
                throttleInput = 0f;
                brakeInput = Mathf.Clamp01(brakeInput);
            }

            if (Mathf.Abs(throttleInput) > 0.05f || Mathf.Abs(brakeInput) > 0.05f)
            {
                fuel = Mathf.Max(0f, fuel - (fuelConsumption * (1f + SpeedKmh / 20f)) * Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            float forwardSpeed = Vector3.Dot(rb.velocity, transform.forward);
            float steeringFactor = Mathf.Clamp01(Mathf.Abs(forwardSpeed) / 12f);

            if (Mathf.Abs(steeringInput) > 0.01f)
            {
                float turnAmount = steeringInput * steeringAngle * steeringSensitivity * steeringFactor * Time.fixedDeltaTime * 2.2f;
                Quaternion rotationDelta = Quaternion.Euler(0f, turnAmount, 0f);
                rb.MoveRotation(rb.rotation * rotationDelta);
            }

            if (transmission == TransmissionMode.Reverse)
            {
                if (throttleInput > 0.05f && fuel > 0f)
                {
                    rb.AddForce(-transform.forward * acceleration * throttleInput * 0.9f, ForceMode.Acceleration);
                }
            }
            else if (transmission == TransmissionMode.Drive)
            {
                if (throttleInput > 0.05f && fuel > 0f)
                {
                    rb.AddForce(transform.forward * acceleration * throttleInput, ForceMode.Acceleration);
                }
            }

            if (brakeInput > 0.05f)
            {
                rb.AddForce(-transform.forward * brakeForce * brakeInput, ForceMode.Acceleration);
            }

            if (isHandbrakeOn)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * 0.8f);
            }

            float maxSpeed = transmission == TransmissionMode.Reverse ? reverseSpeed : maxForwardSpeed;
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            if (localVelocity.z > maxSpeed / 3.6f && transmission == TransmissionMode.Drive)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * (maxSpeed / 3.6f), Time.fixedDeltaTime * 1.5f);
            }
            else if (localVelocity.z < -reverseSpeed / 3.6f && transmission == TransmissionMode.Reverse)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, -transform.forward * (reverseSpeed / 3.6f), Time.fixedDeltaTime * 1.5f);
            }
        }

        public void SetThrottle(float value)
        {
            throttleInput = Mathf.Clamp01(value);
        }

        public void SetBrake(float value)
        {
            brakeInput = Mathf.Clamp01(value);
        }

        public void SetSteering(float value)
        {
            steeringInput = Mathf.Clamp(value, -1f, 1f);
        }

        public void SetHandbrake(bool value)
        {
            isHandbrakeOn = value;
        }

        public void ToggleLights()
        {
            headlightsOn = !headlightsOn;
        }

        public void ToggleHighBeam()
        {
            highBeamOn = !highBeamOn;
        }

        public void ToggleIndicator(bool left)
        {
            if (left)
            {
                leftIndicatorOn = !leftIndicatorOn;
                if (leftIndicatorOn)
                {
                    rightIndicatorOn = false;
                }
            }
            else
            {
                rightIndicatorOn = !rightIndicatorOn;
                if (rightIndicatorOn)
                {
                    leftIndicatorOn = false;
                }
            }
        }

        public void ToggleHazards()
        {
            hazardsOn = !hazardsOn;
            if (hazardsOn)
            {
                leftIndicatorOn = true;
                rightIndicatorOn = true;
            }
            else
            {
                leftIndicatorOn = false;
                rightIndicatorOn = false;
            }
        }

        public void ToggleHorn()
        {
            hornActive = !hornActive;
        }

        public void ShiftNeutral()
        {
            transmission = TransmissionMode.Neutral;
        }

        public void ShiftDrive()
        {
            transmission = TransmissionMode.Drive;
        }

        public void ShiftReverse()
        {
            transmission = TransmissionMode.Reverse;
        }

        public void ToggleDoorState()
        {
            doorsOpen = !doorsOpen;
        }

        public void ToggleLuggage()
        {
            luggageOpen = !luggageOpen;
        }

        private void ReadKeyboardFallback()
        {
            if (Application.isEditor || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.LinuxEditor)
            {
                throttleInput = Mathf.Clamp01(Mathf.Max(throttleInput, Input.GetAxis("Vertical")));
                brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
                steeringInput = Input.GetAxis("Horizontal");

                if (Input.GetKeyDown(KeyCode.R))
                {
                    ShiftReverse();
                }
                if (Input.GetKeyDown(KeyCode.N))
                {
                    ShiftNeutral();
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    ShiftDrive();
                }
                if (Input.GetKeyDown(KeyCode.L))
                {
                    ToggleLights();
                }
                if (Input.GetKeyDown(KeyCode.H))
                {
                    ToggleHorn();
                }
                if (Input.GetKeyDown(KeyCode.B))
                {
                    isHandbrakeOn = !isHandbrakeOn;
                }
            }
        }

        private void UpdateEngineRpm()
        {
            engineRpm = Mathf.Lerp(engineRpm, Mathf.Clamp01(Mathf.Abs(throttleInput) + Mathf.Abs(brakeInput)) * 2200f + Mathf.Abs(SpeedKmh) * 12f, Time.deltaTime * 2f);
        }
    }
}
