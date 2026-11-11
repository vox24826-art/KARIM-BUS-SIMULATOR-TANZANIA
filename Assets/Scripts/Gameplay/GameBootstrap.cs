using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private bool autoBuildOnStart = true;

        private static MaterialPropertyBlock materialBlock;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void EnsureRuntimeBootstrap()
        {
            if (FindAnyObjectByType<GameBootstrap>() == null)
            {
                new GameObject("GameBootstrap").AddComponent<GameBootstrap>();
            }
        }

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

            var routeSystem = new GameObject("RouteSystem").AddComponent<RouteSystem>();
            routeSystem.CreateDefaultRoute();
            CreateGround();
            CreateRoad(routeSystem.routeStops);

            BusController bus = CreateBus();
            routeSystem.bus = bus;
            var passengers = new GameObject("PassengerSystem").AddComponent<PassengerSystem>();
            passengers.waitingPassengers = 42;
            routeSystem.passengerSystem = passengers;

            var progression = new GameObject("RouteProgression").AddComponent<RouteProgressionManager>();
            progression.InitializeDefaults();
            var career = new GameObject("DriverCareer").AddComponent<DriverCareer>();
            var missions = new GameObject("MissionManager").AddComponent<MissionManager>();
            var routeGameplay = new GameObject("RouteGameplay").AddComponent<RouteGameplayController>();
            routeGameplay.progression = progression;
            routeGameplay.busController = bus;
            routeGameplay.driverCareer = career;
            routeGameplay.missionManager = missions;
            routeGameplay.BeginRoute("DAR_ES_SALAAM_TO_ARUSHA");
            routeSystem.OnRouteCompleted += () => routeGameplay.FinishRoute(true, passengers.satisfaction);

            CreateStations(routeSystem.routeStops);
            CreateScenery();
            CreateTraffic();
            CreateWeather();
            new GameObject("GraphicsQualityManager").AddComponent<GraphicsQualityManager>();
            CameraFollowController cameraController = CreateCamera(bus);
            CreateHUD(bus, routeSystem, passengers);
            CreateTouchControls(bus, cameraController);

            Debug.Log("Dar es Salaam to Arusha prototype route ready.");
        }

        private void CreateGround()
        {
            var ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.name = "TanzanianTerrain";
            ground.transform.localScale = new Vector3(140f, 1f, 100f);
            ground.transform.position = new Vector3(0f, -0.12f, 380f);
            SetColor(ground.GetComponent<Renderer>(), new Color(0.42f, 0.53f, 0.28f, 1f));
        }

        private void CreateRoad(List<RouteStop> stops)
        {
            var road = GameObject.CreatePrimitive(PrimitiveType.Cube);
            road.name = "DarArushaHighway";
            float routeEnd = stops[stops.Count - 1].position.z;
            road.transform.position = new Vector3(0f, -0.015f, routeEnd * 0.5f);
            road.transform.localScale = new Vector3(18f, 0.2f, routeEnd + 30f);
            SetColor(road.GetComponent<Renderer>(), new Color(0.16f, 0.18f, 0.17f, 1f));

            for (float z = 8f; z < routeEnd; z += 22f)
            {
                CreateRoadMarking(new Vector3(0f, 0.09f, z), new Vector3(0.18f, 0.025f, 7f), new Color(0.95f, 0.78f, 0.22f, 1f));
            }

            CreateRoadMarking(new Vector3(-8.55f, 0.09f, routeEnd * 0.5f), new Vector3(0.16f, 0.025f, routeEnd), Color.white);
            CreateRoadMarking(new Vector3(8.55f, 0.09f, routeEnd * 0.5f), new Vector3(0.16f, 0.025f, routeEnd), Color.white);
        }

        private void CreateRoadMarking(Vector3 position, Vector3 scale, Color color)
        {
            var marking = GameObject.CreatePrimitive(PrimitiveType.Cube);
            marking.name = "RoadMarking";
            marking.transform.position = position;
            marking.transform.localScale = scale;
            SetColor(marking.GetComponent<Renderer>(), color);
            marking.GetComponent<Collider>().enabled = false;
        }

        private BusController CreateBus()
        {
            var busObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            busObject.name = "ScaniaCoach";
            busObject.transform.position = new Vector3(-4.5f, 1.7f, 0f);
            busObject.transform.localScale = new Vector3(2.8f, 3.1f, 9.4f);
            SetColor(busObject.GetComponent<Renderer>(), new Color(0.78f, 0.82f, 0.79f, 1f));

            var body = busObject.AddComponent<Rigidbody>();
            body.mass = 14000f;
            body.centerOfMass = new Vector3(0f, -0.45f, 0f);

            var controller = busObject.AddComponent<BusController>();
            controller.maxForwardSpeed = 100f;
            controller.reverseSpeed = 18f;
            controller.acceleration = 1.5f;
            controller.brakeForce = 7f;
            controller.steeringSensitivity = 1f;

            CreateBusPart(busObject.transform, "FrontWindshield", new Vector3(0f, 0.72f, 4.72f), new Vector3(2.3f, 1.05f, 0.08f), new Color(0.12f, 0.24f, 0.28f));
            for (int i = 0; i < 5; i++)
            {
                float windowZ = -3.25f + i * 1.5f;
                CreateBusPart(busObject.transform, "LeftWindow", new Vector3(-1.41f, 0.72f, windowZ), new Vector3(0.06f, 0.88f, 1.18f), new Color(0.15f, 0.28f, 0.32f));
                CreateBusPart(busObject.transform, "RightWindow", new Vector3(1.41f, 0.72f, windowZ), new Vector3(0.06f, 0.88f, 1.18f), new Color(0.15f, 0.28f, 0.32f));
            }

            CreateBusPart(busObject.transform, "FrontBumper", new Vector3(0f, -0.48f, 4.73f), new Vector3(2.35f, 0.12f, 0.12f), new Color(0.12f, 0.18f, 0.2f));
            CreateBusPart(busObject.transform, "LiveryStripe", new Vector3(0f, -0.68f, -0.2f), new Vector3(2.84f, 0.18f, 8.6f), new Color(0.8f, 0.24f, 0.12f));
            CreateBusPart(busObject.transform, "Roof", new Vector3(0f, 1.52f, 0f), new Vector3(2.76f, 0.08f, 9.2f), new Color(0.82f, 0.84f, 0.8f));

            for (int side = -1; side <= 1; side += 2)
            {
                for (int axle = -1; axle <= 1; axle += 2)
                {
                    var wheel = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    wheel.name = "CoachWheel";
                    wheel.transform.SetParent(busObject.transform, false);
                    wheel.transform.localPosition = new Vector3(side * 1.42f, -1.08f, axle * 2.75f);
                    wheel.transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
                    wheel.transform.localScale = new Vector3(0.82f, 0.32f, 0.82f);
                    SetColor(wheel.GetComponent<Renderer>(), new Color(0.08f, 0.09f, 0.09f));
                    wheel.GetComponent<Collider>().enabled = false;
                }
            }

            return controller;
        }

        private void CreateBusPart(Transform parent, string partName, Vector3 position, Vector3 scale, Color color)
        {
            var part = GameObject.CreatePrimitive(PrimitiveType.Cube);
            part.name = partName;
            part.transform.SetParent(parent, false);
            part.transform.localPosition = position;
            part.transform.localScale = scale;
            SetColor(part.GetComponent<Renderer>(), color);
            part.GetComponent<Collider>().enabled = false;
        }

        private void CreateStations(List<RouteStop> stops)
        {
            foreach (var stop in stops)
            {
                var marker = GameObject.CreatePrimitive(PrimitiveType.Cube);
                marker.name = "BusStop_" + stop.name.Replace(' ', '_');
                marker.transform.position = stop.position + new Vector3(0f, 0.12f, 0f);
                marker.transform.localScale = new Vector3(5.5f, 0.05f, 9f);
                SetColor(marker.GetComponent<Renderer>(), new Color(0.93f, 0.72f, 0.18f, 1f));
                marker.GetComponent<Collider>().enabled = false;

                CreateTownSign(stop.name, stop.position.z + 14f);
                for (int passenger = 0; passenger < 3; passenger++)
                {
                    var person = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    person.name = "WaitingPassenger";
                    person.transform.position = new Vector3(-11f, 0.9f, stop.position.z + (passenger - 1) * 1.7f);
                    person.transform.localScale = new Vector3(0.45f, 0.9f, 0.45f);
                    SetColor(person.GetComponent<Renderer>(), passenger == 1
                        ? new Color(0.72f, 0.3f, 0.14f)
                        : new Color(0.12f, 0.34f, 0.48f));
                    person.GetComponent<Collider>().enabled = false;
                }
            }
        }

        private void CreateTownSign(string townName, float z)
        {
            var post = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            post.name = "RoadSignPost";
            post.transform.position = new Vector3(12f, 2f, z);
            post.transform.localScale = new Vector3(0.16f, 2f, 0.16f);
            post.GetComponent<Collider>().enabled = false;

            var board = GameObject.CreatePrimitive(PrimitiveType.Cube);
            board.name = "TownSign_" + townName.Replace(' ', '_');
            board.transform.position = new Vector3(12f, 3.8f, z);
            board.transform.localScale = new Vector3(4.5f, 1f, 0.14f);
            SetColor(board.GetComponent<Renderer>(), new Color(0.08f, 0.35f, 0.22f, 1f));
            board.GetComponent<Collider>().enabled = false;

            var label = new GameObject("SignText").AddComponent<TextMesh>();
            label.transform.SetParent(board.transform, false);
            label.transform.localPosition = new Vector3(0f, 0f, -0.09f);
            label.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            label.text = townName;
            label.fontSize = 48;
            label.characterSize = 0.12f;
            label.anchor = TextAnchor.MiddleCenter;
            label.color = Color.white;
        }

        private void CreateScenery()
        {
            for (int i = 0; i < 30; i++)
            {
                float side = i % 2 == 0 ? -1f : 1f;
                float x = side * Random.Range(14f, 42f);
                float z = Random.Range(12f, 748f);
                var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                trunk.name = "RoadsideTreeTrunk";
                trunk.transform.position = new Vector3(x, 2f, z);
                trunk.transform.localScale = new Vector3(0.7f, 2f, 0.7f);
                SetColor(trunk.GetComponent<Renderer>(), new Color(0.35f, 0.24f, 0.13f));
                trunk.GetComponent<Collider>().enabled = false;

                var canopy = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                canopy.name = "RoadsideTreeCanopy";
                canopy.transform.position = new Vector3(x, 5f, z);
                canopy.transform.localScale = Vector3.one * Random.Range(3f, 5f);
                SetColor(canopy.GetComponent<Renderer>(), new Color(0.18f, 0.38f, 0.18f));
                canopy.GetComponent<Collider>().enabled = false;
            }
        }

        private void CreateTraffic()
        {
            for (int i = 0; i < 3; i++)
            {
                CreateTrafficVehicle(-4.5f, 65f + i * 230f, 9f + i, 1, new Color(0.18f, 0.4f, 0.62f));
                CreateTrafficVehicle(4.5f, 155f + i * 210f, 12f + i, -1, new Color(0.72f, 0.29f, 0.12f));
            }
        }

        private void CreateTrafficVehicle(float x, float z, float speed, int direction, Color color)
        {
            var vehicle = GameObject.CreatePrimitive(PrimitiveType.Cube);
            vehicle.name = direction > 0 ? "TanzanianHighwayTraffic" : "OncomingHighwayTraffic";
            vehicle.transform.position = new Vector3(x, 0.82f, z);
            vehicle.transform.localScale = new Vector3(1.9f, 1.5f, 4.2f);
            SetColor(vehicle.GetComponent<Renderer>(), color);
            var body = vehicle.AddComponent<Rigidbody>();
            body.isKinematic = true;
            body.interpolation = RigidbodyInterpolation.Interpolate;
            var traffic = vehicle.AddComponent<TrafficVehicleController>();
            traffic.speed = speed;
            traffic.direction = direction;
        }

        private CameraFollowController CreateCamera(BusController bus)
        {
            Camera camera = Camera.main;
            if (camera == null)
            {
                var cameraObject = new GameObject("Main Camera");
                cameraObject.tag = "MainCamera";
                camera = cameraObject.AddComponent<Camera>();
                cameraObject.AddComponent<AudioListener>();
            }

            camera.fieldOfView = 65f;
            var cameraController = camera.GetComponent<CameraFollowController>();
            if (cameraController == null)
            {
                cameraController = camera.gameObject.AddComponent<CameraFollowController>();
            }
            cameraController.target = bus.transform;
            return cameraController;
        }

        private void CreateTouchControls(BusController bus, CameraFollowController cameraController)
        {
            var controlsObject = new GameObject("TouchControls");
            var controls = controlsObject.AddComponent<TouchControlPanel>();
            controls.BindBus(bus);
            controls.cameraController = cameraController;
            controls.CreateDefaultLayout();
        }

        private void CreateWeather()
        {
            var weather = new GameObject("WeatherSystem").AddComponent<WeatherSystem>();
            var sun = new GameObject("Sun");
            sun.transform.rotation = Quaternion.Euler(40f, 20f, 0f);
            weather.sunLight = sun.AddComponent<Light>();
            weather.sunLight.type = LightType.Directional;
            weather.sunLight.intensity = 1.2f;
            weather.sunLight.color = new Color(1f, 0.9f, 0.75f);
            weather.ApplyWeather(WeatherCondition.Sunny);
        }

        private void CreateHUD(BusController bus, RouteSystem routeSystem, PassengerSystem passengers)
        {
            var canvasObject = new GameObject("HUDCanvas");
            var canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;
            canvasObject.AddComponent<GraphicRaycaster>();

            var hud = canvasObject.AddComponent<BusHUD>();
            hud.bus = bus;
            hud.routeSystem = routeSystem;
            hud.passengerSystem = passengers;
            hud.speedText = CreateText(canvasObject.transform, "SpeedText", "0 km/h", new Vector2(0.03f, 0.95f), 32, TextAnchor.MiddleLeft);
            hud.rpmText = CreateText(canvasObject.transform, "RpmText", "RPM: 0", new Vector2(0.03f, 0.89f), 22, TextAnchor.MiddleLeft);
            hud.gearText = CreateText(canvasObject.transform, "GearText", "D", new Vector2(0.96f, 0.95f), 34, TextAnchor.MiddleRight);
            hud.fuelText = CreateText(canvasObject.transform, "FuelText", "Fuel: 100%", new Vector2(0.96f, 0.89f), 22, TextAnchor.MiddleRight);
            hud.routeText = CreateText(canvasObject.transform, "RouteText", "Next stop: Dar es Salaam", new Vector2(0.5f, 0.06f), 24, TextAnchor.MiddleCenter);
            hud.passengerText = CreateText(canvasObject.transform, "PassengerText", "Passengers: " + passengers.boardedPassengers, new Vector2(0.5f, 0.95f), 20, TextAnchor.MiddleCenter);
        }

        private Text CreateText(Transform parent, string name, string text, Vector2 anchor, int fontSize, TextAnchor alignment)
        {
            var textObject = new GameObject(name);
            textObject.transform.SetParent(parent, false);
            var rect = textObject.AddComponent<RectTransform>();
            rect.anchorMin = anchor;
            rect.anchorMax = anchor;
            rect.pivot = alignment == TextAnchor.MiddleLeft ? new Vector2(0f, 0.5f)
                : alignment == TextAnchor.MiddleRight ? new Vector2(1f, 0.5f) : new Vector2(0.5f, 0.5f);
            rect.sizeDelta = new Vector2(650f, 60f);

            var uiText = textObject.AddComponent<Text>();
            uiText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            uiText.text = text;
            uiText.fontSize = fontSize;
            uiText.alignment = alignment;
            uiText.color = Color.white;
            uiText.resizeTextForBestFit = true;
            return uiText;
        }

        private static void SetColor(Renderer renderer, Color color)
        {
            if (materialBlock == null)
            {
                materialBlock = new MaterialPropertyBlock();
            }

            materialBlock.SetColor("_Color", color);
            materialBlock.SetColor("_BaseColor", color);
            renderer.SetPropertyBlock(materialBlock);
        }
    }
}