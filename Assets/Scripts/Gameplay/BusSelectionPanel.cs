using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class BusSelectionPanel : MonoBehaviour
    {
        public BusCatalog busCatalog;
        public List<BusDefinition> availableBuses = new List<BusDefinition>();

        private void Start()
        {
            if (busCatalog == null)
            {
                busCatalog = FindAnyObjectByType<BusCatalog>();
            }

            if (busCatalog != null)
            {
                busCatalog.BuildDefaultCatalog();
                availableBuses = busCatalog.buses;
            }
        }

        public void BuildBusSelectionUI()
        {
            var canvasObject = new GameObject("BusSelectionCanvas");
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
            titleText.text = "CHOOSE BUS";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 26;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = Color.white;

            int index = 0;
            foreach (var bus in availableBuses)
            {
                var panel = new GameObject("BusPanel_" + bus.id);
                panel.transform.SetParent(canvasObject.transform);
                var rect = panel.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.12f, 0.8f - index * 0.18f);
                rect.anchorMax = new Vector2(0.88f, 0.88f - index * 0.18f);
                rect.sizeDelta = new Vector2(0f, 0f);

                var image = panel.AddComponent<Image>();
                image.color = new Color(0.1f, 0.22f, 0.42f, 1f);

                var text = new GameObject("BusInfo");
                text.transform.SetParent(panel.transform);
                var textRect = text.AddComponent<RectTransform>();
                textRect.anchorMin = Vector2.zero;
                textRect.anchorMax = Vector2.one;
                textRect.offsetMin = Vector2.zero;
                textRect.offsetMax = Vector2.zero;
                var busText = text.AddComponent<Text>();
                busText.text = bus.busName + " | Seats: " + bus.seats + " | Price: $" + bus.price;
                busText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                busText.fontSize = 18;
                busText.alignment = TextAnchor.MiddleCenter;
                busText.color = Color.white;
                index++;
            }
        }
    }
}
