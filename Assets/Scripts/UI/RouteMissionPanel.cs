using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class RouteMissionPanel : MonoBehaviour
    {
        public MissionManager missionManager;
        public RouteProgressionManager routeProgressionManager;

        private void Start()
        {
            if (missionManager == null)
            {
                missionManager = FindAnyObjectByType<MissionManager>();
            }

            if (routeProgressionManager == null)
            {
                routeProgressionManager = FindAnyObjectByType<RouteProgressionManager>();
            }
        }

        public void BuildMissionUI()
        {
            var canvasObject = new GameObject("MissionCanvas");
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
            titleText.text = "MISSIONS & ROUTES";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 26;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = Color.white;

            var route = routeProgressionManager != null ? routeProgressionManager.GetCurrentRoute() : null;
            var textObj = new GameObject("CurrentRoute");
            textObj.transform.SetParent(canvasObject.transform);
            var textRect = textObj.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0.2f, 0.8f);
            textRect.anchorMax = new Vector2(0.8f, 0.88f);
            var currentText = textObj.AddComponent<Text>();
            currentText.text = route != null ? "Current route: " + route.routeName : "Current route: Dar es Salaam -> Arusha";
            currentText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            currentText.fontSize = 20;
            currentText.color = new Color(0.8f, 0.9f, 1f, 1f);

            var missionList = missionManager != null ? missionManager.missions : new System.Collections.Generic.List<MissionDefinition>();
            for (int i = 0; i < Mathf.Min(missionList.Count, 5); i++)
            {
                var missionGo = new GameObject("Mission_" + i);
                missionGo.transform.SetParent(canvasObject.transform);
                var missionRect = missionGo.AddComponent<RectTransform>();
                missionRect.anchorMin = new Vector2(0.12f, 0.72f - i * 0.12f);
                missionRect.anchorMax = new Vector2(0.88f, 0.82f - i * 0.12f);
                var missionText = missionGo.AddComponent<Text>();
                missionText.text = missionList[i].title + " - " + (missionList[i].completed ? "Completed" : "Available");
                missionText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                missionText.fontSize = 18;
                missionText.color = missionList[i].completed ? new Color(0.4f, 1f, 0.5f, 1f) : Color.white;
            }
        }
    }
}
