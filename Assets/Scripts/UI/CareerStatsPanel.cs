using UnityEngine;
using UnityEngine.UI;

namespace KarimBusSimulator
{
    public class CareerStatsPanel : MonoBehaviour
    {
        public DriverCareer career;

        private void Start()
        {
            if (career == null)
            {
                career = FindAnyObjectByType<DriverCareer>();
            }
        }

        public void BuildStatsUI()
        {
            var canvasObject = new GameObject("CareerStatsCanvas");
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
            titleText.text = "CAREER STATS";
            titleText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            titleText.fontSize = 26;
            titleText.alignment = TextAnchor.MiddleCenter;
            titleText.color = Color.white;

            var stats = new[]
            {
                ("Driver Level", career != null ? career.driverLevel.ToString() : "1"),
                ("Money", career != null ? "$" + career.money : "$2500"),
                ("Trips", career != null ? career.completedTrips.ToString() : "0"),
                ("Distance", career != null ? career.totalDistanceKm.ToString("F0") + " km" : "0 km"),
                ("Reputation", career != null ? career.reputation.ToString() : "0"),
                ("Safety", career != null ? career.safetyRating.ToString("F0") + "%" : "80%")
            };

            for (int i = 0; i < stats.Length; i++)
            {
                var statGo = new GameObject("Stat_" + i);
                statGo.transform.SetParent(canvasObject.transform);
                var rect = statGo.AddComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.28f, 0.8f - i * 0.1f);
                rect.anchorMax = new Vector2(0.72f, 0.85f - i * 0.1f);
                rect.sizeDelta = new Vector2(400f, 40f);

                var text = statGo.AddComponent<Text>();
                text.text = stats[i].Item1 + ": " + stats[i].Item2;
                text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
                text.fontSize = 18;
                text.color = new Color(0.85f, 0.9f, 1f, 1f);
            }
        }
    }
}
