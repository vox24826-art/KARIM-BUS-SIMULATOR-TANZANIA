using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private bool autoBuildOnStart = true;

        private void Start()
        {
            if (autoBuildOnStart)
            {
                BuildPrototypeWorld();
            }
        }

        [ContextMenu("Build Prototype World")]
        public void BuildPrototypeWorld()
        {
            if (FindAnyObjectByType<BusController>() != null)
            {
                return;
            }

            CreateGround();
            CreateRoad();
            var bus = CreateBus();
            CreateStations();
            CreateWeather();
            CreateHUD(bus);

            var saveData = SaveSystem.Load();
            Debug.Log("Loaded save data: " + saveData.currentRoute);
        }

        private GameObject CreateGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "Ground";
            ground.transform.localScale = new Vector3(20f, 1f, 40f);
            ground.transform.position = new Vector3(0f, 0f, 380f);
            return ground;
        }

        private GameObject CreateRoad()
        {
            var road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = "Road";
            road.transform.position = new Vector3(0f, 0.05f, 380f);
            road.transform.localScale = new Vector3(18f, 0.1f, 78f);

            var roadMarking = GameObject.CreatePrimitive(PrimitiveType.Cube);
            roadMarking.name = "RoadHighlight";
            roadMarking.transform.SetParent(road.transform);
            roadMarking.transform.localPosition = new Vector3(0f, 0.08f, 0f);
            roadMarking.transform.localScale = new Vector3(0.16f, 0.02f, 0.9f);
            var render = roadMarking.GetComponent<Renderer>();
            render.material.color = new Color(0.9f, 0.9f, 0.7f, 1f);

            return road;
        }

        private BusController CreateBus()
        {
            var busObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            busObject.name = "ScaniaCoach";
            busObject.transform.position = new Vector3(0f, 1.75f, 0f);
            busObject.transform.localScale = new Vector3(2.8f, 2.8f, 9.5f);

            var renderer = busObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(0.15f, 0.35f, 0.75f, 1f);
            }

            var rb = busObject.AddComponent<Rigidbody>();
            rb.mass = 7000f;
            rb.drag = 0.08f;

            var controller = busObject.AddComponent<BusController>();
            controller.maxForwardSpeed = 120f;
            controller.reverseSpeed = 26f;
            controller.acceleration = 28f;
            controller.brakeForce = 40f;
            controller.steeringSensitivity = 1.2f;
            controller.headlightsOn = true;

            var frontWindow = CreateWindowSegment(busObject.transform, new Vector3(0f, 1.4f, 2.7f), new Vector3(2.5f, 1.1f, 2.8f));
            frontWindow.name = "FrontWindshield";
            var sideWindowLeft = CreateWindowSegment(busObject.transform, new Vector3(-1.3f, 1.35f, 0f), new Vector3(0.15f, 1f, 8.2f));
            sideWindowLeft.name = "LeftSideWindows";
            var sideWindowRight = CreateWindowSegment(busObject.transform, new Vector3(1.3f, 1.35f, 0f), new Vector3(0.15f, 1f, 8.2f));
            sideWindowRight.name = "RightSideWindows";

            var steeringWheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            steeringWheel.transform.SetParent(busObject.transform);
            steeringWheel.transform.localPosition = new Vector3(0.8f, 1.1f, 1.7f);
            steeringWheel.transform.localScale = new Vector3(0.55f, 0.06f, 0.55f);
            steeringWheel.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            return controller;
        }

        private GameObject CreateWindowSegment(Transform parent, Vector3 localPosition, Vector3 localScale)
        {
            var segment = GameObject.CreatePrimitive(PrimitiveType.Cube);
            segment.transform.SetParent(parent);
            segment.transform.localPosition = localPosition;
            segment.transform.localScale = localScale;

            var renderer = segment.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material.color = new Color(0.8f, 0.9f, 1f, 0.8f);
            }

            return segment;
        }

        private void CreateStations()
        {
            var routeSystem = new GameObject("RouteSystem").AddComponent<RouteSystem>();
            routeSystem.CreateDefaultRoute();
            routeSystem.bus = FindAnyObjectByType<BusController>();

            var passengerSystem = new GameObject("PassengerSystem").AddComponent<PassengerSystem>();
            passengerSystem.waitingPassengers = 26;
            routeSystem.passengerSystem = passengerSystem;

            var stops = new List<RouteStop>(routeSystem.routeStops);
            foreach (var stop in stops)
            {
                var marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                marker.name = "Station_" + stop.name.Replace(' ', '_');
                marker.transform.position = stop.position + new Vector3(0f, 0.5f, 0f);
                marker.transform.localScale = new Vector3(8f, 0.8f, 3f);
                marker.GetComponent<Renderer>().material.color = new Color(0.95f, 0.7f, 0.1f, 1f);
            }
        }

        private void CreateWeather()
        {
            var weatherHolder = new GameObject("WeatherSystem");
            var weather = weatherHolder.AddComponent<WeatherSystem>();
            var sun = new GameObject("Sun");
            sun.transform.rotation = Quaternion.Euler(40f, 20f, 0f);
            weather.sunLight = sun.AddComponent<Light>();
            weather.sunLight.type = LightType.Directional;
            weather.sunLight.intensity = 1.2f;
            weather.sunLight.color = new Color(1f, 0.9f, 0.75f);
            weather.ApplyWeather(WeatherCondition.Sunny);
        }

        private void CreateHUD(BusController bus)
        {
            var canvasObject = new GameObject("HUDCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            var hud = canvasObject.AddComponent<BusHUD>();
            hud.bus = bus;
            hud.speedText = CreateText(canvasObject.transform, "SpeedText", "0 km/h", new Vector2(0.05f, 0.9f), 30, TextAnchor.MiddleLeft);
            hud.rpmText = CreateText(canvasObject.transform, "RpmText", "RPM: 0", new Vector2(0.05f, 0.82f), 24, TextAnchor.MiddleLeft);
            hud.gearText = CreateText(canvasObject.transform, "GearText", "D", new Vector2(0.12f, 0.5f), 36, TextAnchor.MiddleCenter);
            hud.fuelText = CreateText(canvasObject.transform, "FuelText", "Fuel: 100%", new Vector2(0.82f, 0.92f), 24, TextAnchor.MiddleRight);
            hud.routeText = CreateText(canvasObject.transform, "RouteText", "Route: Dar es Salaam -> Arusha", new Vector2(0.5f, 0.12f), 22, TextAnchor.MiddleCenter);
        }

        private Text CreateText(Transform parent, string name, string text, Vector2 anchorPosition, int fontSize, TextAnchor alignment)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent);
            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 1f);
            rect.anchoredPosition = anchorPosition;
            rect.sizeDelta = new Vector2(400f, 90f);

            var uiText = textObject.AddComponent<Text>();
            uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            uiText.text = text;
            uiText.fontSize = fontSize;
            uiText.alignment = alignment;
            uiText.color = Color.white;
            return uiText;
        }
    }
}
