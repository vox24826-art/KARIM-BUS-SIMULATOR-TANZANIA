using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class BusGarageScreen : MonoBehaviour
    {
        public BusGarageManager garageManager;

        private void Start()
        {
            if (garageManager == null)
            {
                garageManager = FindAnyObjectByType<BusGarageManager>();
            }
        }

        public void BuildGarageUI()
        {
            var canvasObject = new GameObject("GarageCanvas");
            canvasObject.AddComponent<Canvas>();
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();

            var title = new GameObject("Title");
            title.transform.SetParent(canvasObject.transform);
            var rect = title.AddComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 0.95f);
            rect.anchorMax = new Vector2(0.5f, 0.95f);
            rect.sizeDelta = new Vector2(500f, 40f);
            var text = title.AddComponent<Text>();
            text.text = "BUS GARAGE";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 28;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;

            var busList = garageManager != null ? garageManager.ownedBuses : null;
            for (int i = 0; i < (busList != null ? busList.Count : 3); i++)
            {
                var busGo = new GameObject("BusSlot_" + i);
                busGo.transform.SetParent(canvasObject.transform);
                var panelRect = busGo.AddComponent<RectTransform>();
                panelRect.anchorMin = new Vector2(0.15f, 0.75f - i * 0.18f);
                panelRect.anchorMax = new Vector2(0.85f, 0.88f - i * 0.18f);
                panelRect.sizeDelta = new Vector2(0f, 0f);

                var image = busGo.AddComponent<Image>();
                image.color = new Color(0.18f, 0.35f, 0.62f, 1f);

                var label = new GameObject("BusName");
                label.transform.SetParent(busGo.transform);
                var labelRect = label.AddComponent<RectTransform>();
                labelRect.anchorMin = Vector2.zero;
                labelRect.anchorMax = Vector2.one;
                labelRect.offsetMin = Vector2.zero;
                labelRect.offsetMax = Vector2.zero;

                var labelText = label.AddComponent<Text>();
                labelText.text = busList != null && busList.Count > i ? busList[i].busName : "Scania Coach";
                labelText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                labelText.fontSize = 22;
                labelText.alignment = TextAnchor.MiddleCenter;
                labelText.color = Color.white;
            }
        }
    }
}
