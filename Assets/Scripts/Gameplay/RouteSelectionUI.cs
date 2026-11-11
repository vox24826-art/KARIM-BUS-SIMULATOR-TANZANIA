using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class RouteSelectionUI : MonoBehaviour
    {
        public RouteProgressionManager progression;
        public List<RouteProgressionManager.RouteDefinition> availableRoutes = new List<RouteProgressionManager.RouteDefinition>();

        private void Start()
        {
            if (progression == null)
            {
                progression = FindAnyObjectByType<RouteProgressionManager>();
            }

            if (progression != null)
            {
                progression.InitializeDefaults();
                availableRoutes = progression.routes;
            }
        }

        public void SelectRoute(string routeId)
        {
            if (progression == null)
            {
                return;
            }

            progression.SetCurrentRoute(routeId);
            Debug.Log("Selected route: " + routeId);
        }

        public void BuildUI()
        {
            var canvasObject = new GameObject("RouteSelectionCanvas");
            canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            var title = new GameObject("Title");
            title.transform.SetParent(canvasObject.transform);
            var titleRect = title.AddComponent<RectTransform>();
            titleRect.anchorMin = new Vector2(0.5f, 0.95f);
            titleRect.anchorMax = new Vector2(0.5f, 0.95f);
            titleRect.sizeDelta = new Vector2(500f, 40f);
            var titleText = title.AddComponent<Text>();
            titleText.text = "SELECT ROUTE";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.fontSize = 26;
            titleText.color = Color.white;

            int index = 0;
            foreach (var route in availableRoutes)
            {
                var buttonObject = new GameObject("RouteButton_" + route.routeId);
                buttonObject.transform.SetParent(canvasObject.transform);
                var rect = buttonObject.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 0.8f - index * 0.1f);
                rect.anchorMax = new Vector2(0.5f, 0.8f - index * 0.1f);
                rect.sizeDelta = new Vector2(420f, 52f);
                rect.anchoredPosition = Vector2.zero;

                var image = buttonObject.AddComponent<Image>();
                image.color = new Color(0.19f, 0.42f, 0.68f, 1f);

                var button = buttonObject.AddComponent<Button>();
                button.onClick.AddListener(() => SelectRoute(route.routeId));

                var label = new GameObject("Label");
                label.transform.SetParent(buttonObject.transform);
                var labelRect = label.AddComponent<RectTransform>();
                labelRect.anchorMin = Vector2.zero;
                labelRect.anchorMax = Vector2.one;
                labelRect.offsetMin = Vector2.zero;
                labelRect.offsetMax = Vector2.zero;
                var text = label.AddComponent<Text>();
                text.text = route.routeName + " | " + route.distanceKm + " km";
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.alignment = TextAnchor.MiddleCenter;
                text.color = Color.white;

                index++;
            }
        }
    }
}
