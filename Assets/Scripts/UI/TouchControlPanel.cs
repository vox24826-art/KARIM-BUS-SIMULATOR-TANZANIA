using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace KarimBusSimulator
{
    public class TouchControlPanel : MonoBehaviour
    {
        public BusController busController;

        public Slider steeringSlider;
        public Slider throttleSlider;
        public Slider brakeSlider;
        public CameraFollowController cameraController;

        private bool leftHeld;
        private bool rightHeld;
        private bool throttleHeld;
        private bool brakeHeld;

        public void BindBus(BusController targetBus)
        {
            busController = targetBus;
            if (busController == null)
            {
                return;
            }
        }

        private void Update()
        {
            if (busController == null)
            {
                return;
            }

            float sliderSteering = steeringSlider != null ? steeringSlider.value : 0f;
            busController.SetSteering(Mathf.Abs(sliderSteering) > 0.01f ? sliderSteering : (rightHeld ? 1f : 0f) - (leftHeld ? 1f : 0f));
            busController.SetThrottle(Mathf.Max(throttleHeld ? 1f : 0f, throttleSlider != null ? throttleSlider.value : 0f));
            busController.SetBrake(Mathf.Max(brakeHeld ? 1f : 0f, brakeSlider != null ? brakeSlider.value : 0f));
        }

        public void ToggleLights()
        {
            if (busController != null)
            {
                busController.ToggleLights();
            }
        }

        public void ToggleHorn()
        {
            if (busController != null)
            {
                busController.ToggleHorn();
            }
        }

        public void ToggleDoorState()
        {
            if (busController != null)
            {
                busController.ToggleDoorState();
            }
        }

        public void ToggleHandbrake()
        {
            if (busController != null)
            {
                busController.SetHandbrake(!busController.isHandbrakeOn);
            }
        }

        public void ShiftDrive()
        {
            if (busController != null)
            {
                busController.ShiftDrive();
            }
        }

        public void ShiftReverse()
        {
            if (busController != null)
            {
                busController.ShiftReverse();
            }
        }

        public void CreateDefaultLayout()
        {
            if (transform.Find("TouchControlsCanvas") != null)
            {
                return;
            }

            var canvasObject = new GameObject("TouchControlsCanvas");
            canvasObject.transform.SetParent(transform, false);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 10;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            if (EventSystem.current == null)
            {
                var eventSystem = new GameObject("EventSystem");
                eventSystem.AddComponent<EventSystem>();
                eventSystem.AddComponent<StandaloneInputModule>();
            }

            CreateHoldButton(canvasObject.transform, "SteerLeft", new Vector2(0.09f, 0.2f), "LEFT", value => leftHeld = value);
            CreateHoldButton(canvasObject.transform, "SteerRight", new Vector2(0.24f, 0.2f), "RIGHT", value => rightHeld = value);
            CreateHoldButton(canvasObject.transform, "BrakeButton", new Vector2(0.73f, 0.2f), "BRAKE", value => brakeHeld = value);
            CreateHoldButton(canvasObject.transform, "ThrottleButton", new Vector2(0.89f, 0.2f), "GAS", value => throttleHeld = value);

            var gearButton = CreateButton(canvasObject.transform, "GearButton", new Vector2(0.53f, 0.9f), new Vector2(150f, 78f), "D / R");
            gearButton.onClick.AddListener(() =>
            {
                if (busController != null)
                {
                    if (busController.IsReverse) busController.ShiftDrive(); else busController.ShiftReverse();
                }
            });
            CreateButton(canvasObject.transform, "LightButton", new Vector2(0.65f, 0.9f), new Vector2(150f, 78f), "LIGHTS").onClick.AddListener(ToggleLights);
            CreateButton(canvasObject.transform, "HornButton", new Vector2(0.77f, 0.9f), new Vector2(150f, 78f), "HORN").onClick.AddListener(ToggleHorn);
            CreateButton(canvasObject.transform, "DoorButton", new Vector2(0.89f, 0.9f), new Vector2(150f, 78f), "DOORS").onClick.AddListener(ToggleDoorState);
            CreateButton(canvasObject.transform, "CameraButton", new Vector2(0.53f, 0.79f), new Vector2(150f, 70f), "VIEW").onClick.AddListener(() => cameraController?.CycleView());
            CreateButton(canvasObject.transform, "HandbrakeButton", new Vector2(0.53f, 0.68f), new Vector2(150f, 70f), "PARK").onClick.AddListener(ToggleHandbrake);
        }

        private Button CreateButton(Transform parent, string name, Vector2 anchor, Vector2 sizeDelta, string label)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.sizeDelta = sizeDelta;

            var image = go.AddComponent<Image>();
            image.color = new Color(0.21f, 0.48f, 0.74f, 0.8f);

            var button = go.AddComponent<Button>();
            var labelGo = new GameObject("Label");
            labelGo.transform.SetParent(go.transform);
            var labelRect = labelGo.AddComponent<RectTransform>();
            labelRect.anchorMin = Vector2.zero;
            labelRect.anchorMax = Vector2.one;
            labelRect.offsetMin = Vector2.zero;
            labelRect.offsetMax = Vector2.zero;

            var text = labelGo.AddComponent<Text>();
            text.text = label;
            text.alignment = TextAnchor.MiddleCenter;
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 22;
            text.color = Color.white;
            text.resizeTextForBestFit = true;

            return button;
        }

        private void CreateHoldButton(Transform parent, string name, Vector2 anchor, string label, Action<bool> onHold)
        {
            var button = CreateButton(parent, name, anchor, new Vector2(150f, 132f), label);
            var trigger = button.gameObject.AddComponent<EventTrigger>();
            AddPointerEvent(trigger, EventTriggerType.PointerDown, () => onHold(true));
            AddPointerEvent(trigger, EventTriggerType.PointerUp, () => onHold(false));
            AddPointerEvent(trigger, EventTriggerType.PointerExit, () => onHold(false));
        }

        private void AddPointerEvent(EventTrigger trigger, EventTriggerType eventType, Action callback)
        {
            var entry = new EventTrigger.Entry { eventID = eventType };
            entry.callback.AddListener(_ => callback());
            trigger.triggers.Add(entry);
        }
    }
}
