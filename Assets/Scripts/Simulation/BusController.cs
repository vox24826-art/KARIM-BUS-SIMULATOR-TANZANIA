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
        public float maxForwardSpeed = 105f;
        public float reverseSpeed = 18f;
        public float acceleration = 2.4f;
        public float brakeForce = 6.5f;
        public float steeringAngle = 30f;
        public float steeringSensitivity = 1f;
        public float maxFuel = 300f;
        public float fuelConsumption = 0.32f;
        public float massKg = 14000f;
        public float wheelbase = 6.2f;
        public float lateralGrip = 4f;

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
        private float touchSteeringInput;
        private float touchThrottleInput;
        private float touchBrakeInput;
        private float keyboardSteeringInput;
        private float keyboardThrottleInput;
        private float keyboardBrakeInput;

        public float SpeedKmh => Mathf.Abs(Vector3.Dot(rb.velocity, transform.forward)) * 3.6f;
        public float Fuel => fuel;
        public float EngineRpm => engineRpm;
        public string GearLabel => transmission == TransmissionMode.Reverse ? "R" : transmission == TransmissionMode.Neutral ? "N" : "D";
        public bool IsReverse => transmission == TransmissionMode.Reverse;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.mass = massKg;
            rb.drag = 0.12f;
            rb.angularDrag = 2f;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
            fuel = maxFuel;
        }

        private void Update()
        {
            ReadKeyboardFallback();
            steeringInput = Mathf.Abs(touchSteeringInput) > 0.01f ? touchSteeringInput : keyboardSteeringInput;
            throttleInput = Mathf.Max(touchThrottleInput, keyboardThrottleInput);
            brakeInput = Mathf.Max(touchBrakeInput, keyboardBrakeInput);
            UpdateEngineRpm();

            if (fuel <= 0f)
            {
                throttleInput = 0f;
                brakeInput = Mathf.Clamp01(brakeInput);
            }

            if (Mathf.Abs(throttleInput) > 0.05f || Mathf.Abs(brakeInput) > 0.05f)
            {
                float fuelPerHour = fuelConsumption * Mathf.Max(SpeedKmh, 10f) * (1f + throttleInput * 0.35f);
                fuel = Mathf.Max(0f, fuel - fuelPerHour / 3600f * Time.deltaTime);
            }
        }

        private void FixedUpdate()
        {
            Vector3 localVelocity = transform.InverseTransformDirection(rb.velocity);
            if (Mathf.Abs(steeringInput) > 0.01f && Mathf.Abs(localVelocity.z) > 0.5f)
            {
                float steeringRadians = steeringInput * steeringAngle * steeringSensitivity * Mathf.Deg2Rad;
                float yawRate = localVelocity.z / Mathf.Max(1f, wheelbase) * Mathf.Tan(steeringRadians);
                Quaternion rotationDelta = Quaternion.Euler(0f, yawRate * Mathf.Rad2Deg * Time.fixedDeltaTime, 0f);
                rb.MoveRotation(rb.rotation * rotationDelta);
            }

            float forwardLimit = maxForwardSpeed / 3.6f;
            float reverseLimit = reverseSpeed / 3.6f;
            if (transmission == TransmissionMode.Reverse)
            {
                if (throttleInput > 0.05f && fuel > 0f)
                {
                    rb.AddForce(-transform.forward * acceleration * throttleInput * 0.65f, ForceMode.Acceleration);
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
                if (rb.velocity.sqrMagnitude > 0.04f)
                {
                    rb.AddForce(-rb.velocity.normalized * brakeForce * brakeInput, ForceMode.Acceleration);
                }
            }

            rb.AddForce(-transform.right * localVelocity.x * lateralGrip, ForceMode.Acceleration);
            if (isHandbrakeOn)
            {
                rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, Time.fixedDeltaTime * 1.8f);
            }

            localVelocity = transform.InverseTransformDirection(rb.velocity);
            localVelocity.z = Mathf.Clamp(localVelocity.z, -reverseLimit, forwardLimit);
            rb.velocity = transform.TransformDirection(localVelocity);
        }

        public void SetThrottle(float value)
        {
            touchThrottleInput = Mathf.Clamp01(value);
        }

        public void SetBrake(float value)
        {
            touchBrakeInput = Mathf.Clamp01(value);
        }

        public void SetSteering(float value)
        {
            touchSteeringInput = Mathf.Clamp(value, -1f, 1f);
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
                float verticalInput = Input.GetAxis("Vertical");
                keyboardThrottleInput = Mathf.Clamp01(verticalInput);
                keyboardBrakeInput = Input.GetKey(KeyCode.Space) ? 1f : Mathf.Clamp01(-verticalInput);
                keyboardSteeringInput = Input.GetAxis("Horizontal");

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
            else
            {
                keyboardThrottleInput = 0f;
                keyboardBrakeInput = 0f;
                keyboardSteeringInput = 0f;
            }
        }

        private void UpdateEngineRpm()
        {
            engineRpm = Mathf.Lerp(engineRpm, Mathf.Clamp01(Mathf.Abs(throttleInput) + Mathf.Abs(brakeInput)) * 2200f + Mathf.Abs(SpeedKmh) * 12f, Time.deltaTime * 2f);
        }
    }
}
