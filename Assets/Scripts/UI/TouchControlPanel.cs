using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class TouchControlPanel : MonoBehaviour
    {
        public BusController busController;

        public Slider steeringSlider;
        public Slider throttleSlider;
        public Slider brakeSlider;

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

            if (steeringSlider != null)
            {
                busController.SetSteering(steeringSlider.value);
            }

            if (throttleSlider != null)
            {
                busController.SetThrottle(throttleSlider.value);
            }

            if (brakeSlider != null)
            {
                busController.SetBrake(brakeSlider.value);
            }
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
            var canvasObject = new GameObject("TouchControlsCanvas");
            canvasObject.transform.SetParent(transform);
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            var steeringGo = new GameObject("SteeringSlider");
            steeringGo.transform.SetParent(canvasObject.transform);
            var steeringRect = steeringGo.AddComponent<RectTransform>();
            steeringRect.anchorMin = new Vector2(0.06f, 0.12f);
            steeringRect.anchorMax = new Vector2(0.3f, 0.22f);
            steeringRect.offsetMin = Vector2.zero;
            steeringRect.offsetMax = Vector2.zero;
            steeringSlider = steeringGo.AddComponent<Slider>();
            steeringSlider.minValue = -1f;
            steeringSlider.maxValue = 1f;
            steeringSlider.wholeNumbers = false;

            var throttleGo = new GameObject("ThrottleSlider");
            throttleGo.transform.SetParent(canvasObject.transform);
            var throttleRect = throttleGo.AddComponent<RectTransform>();
            throttleRect.anchorMin = new Vector2(0.72f, 0.12f);
            throttleRect.anchorMax = new Vector2(0.9f, 0.22f);
            throttleRect.offsetMin = Vector2.zero;
            throttleRect.offsetMax = Vector2.zero;
            throttleSlider = throttleGo.AddComponent<Slider>();
            throttleSlider.minValue = 0f;
            throttleSlider.maxValue = 1f;
            throttleSlider.wholeNumbers = false;

            var brakeGo = new GameObject("BrakeSlider");
            brakeGo.transform.SetParent(canvasObject.transform);
            var brakeRect = brakeGo.AddComponent<RectTransform>();
            brakeRect.anchorMin = new Vector2(0.72f, 0.25f);
            brakeRect.anchorMax = new Vector2(0.9f, 0.35f);
            brakeRect.offsetMin = Vector2.zero;
            brakeRect.offsetMax = Vector2.zero;
            brakeSlider = brakeGo.AddComponent<Slider>();
            brakeSlider.minValue = 0f;
            brakeSlider.maxValue = 1f;
            brakeSlider.wholeNumbers = false;

            var hornButton = CreateButton(canvasObject.transform, "HornButton", new Vector2(0.84f, 0.72f), new Vector2(0.12f, 0.12f), "Horn");
            hornButton.onClick.AddListener(ToggleHorn);

            var doorButton = CreateButton(canvasObject.transform, "DoorButton", new Vector2(0.68f, 0.72f), new Vector2(0.12f, 0.12f), "Doors");
            doorButton.onClick.AddListener(ToggleDoorState);

            var lightButton = CreateButton(canvasObject.transform, "LightButton", new Vector2(0.52f, 0.72f), new Vector2(0.12f, 0.12f), "Lights");
            lightButton.onClick.AddListener(ToggleLights);
        }

        private Button CreateButton(Transform parent, string name, Vector2 anchoredPosition, Vector2 sizeDelta, string label)
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent);
            var rect = go.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = anchoredPosition;
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
            text.color = Color.white;
            text.resizeTextForBestFit = true;

            return button;
        }
    }
}
