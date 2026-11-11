using UnityEngine;

namespace KarimBusSimulator
{
    public enum WeatherCondition
    {
        Sunny,
        Cloudy,
        Rain,
        HeavyRain
    }

    public class WeatherSystem : MonoBehaviour
    {
        public Light sunLight;
        public ParticleSystem rainParticles;
        public float cycleDuration = 180f;
        public WeatherCondition currentWeather = WeatherCondition.Sunny;

        private float timer;

        private void Start()
        {
            if (sunLight == null)
            {
                sunLight = FindAnyObjectByType<Light>();
            }

            if (rainParticles == null)
            {
                CreateRainParticles();
            }

            ApplyWeather(currentWeather);
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= cycleDuration)
            {
                timer = 0f;
                AdvanceWeather();
            }

            if (sunLight != null)
            {
                sunLight.transform.rotation = Quaternion.Euler(Time.time * 8f, 180f, 0f);
                var intensity = Mathf.Clamp01(0.3f + Mathf.Sin(Time.time * 0.09f) * 0.7f);
                sunLight.intensity = intensity;
            }
        }

        public void AdvanceWeather()
        {
            var next = (int)currentWeather + 1;
            if (next > (int)WeatherCondition.HeavyRain)
            {
                next = (int)WeatherCondition.Sunny;
            }

            ApplyWeather((WeatherCondition)next);
        }

        public void ApplyWeather(WeatherCondition weather)
        {
            currentWeather = weather;

            if (rainParticles != null)
            {
                var emission = rainParticles.emission;
                var rate = weather == WeatherCondition.Rain ? 120f : weather == WeatherCondition.HeavyRain ? 260f : 0f;
                emission.rateOverTime = rate;
                rainParticles.gameObject.SetActive(rate > 0f);
            }

            if (sunLight != null)
            {
                switch (weather)
                {
                    case WeatherCondition.Sunny:
                        RenderSettings.ambientLight = new Color(0.72f, 0.72f, 0.72f);
                        break;
                    case WeatherCondition.Cloudy:
                        RenderSettings.ambientLight = new Color(0.5f, 0.5f, 0.55f);
                        break;
                    case WeatherCondition.Rain:
                        RenderSettings.ambientLight = new Color(0.42f, 0.42f, 0.48f);
                        break;
                    case WeatherCondition.HeavyRain:
                        RenderSettings.ambientLight = new Color(0.32f, 0.33f, 0.38f);
                        break;
                }
            }
        }

        private void CreateRainParticles()
        {
            var rainObject = new GameObject("RainParticles");
            rainObject.transform.SetParent(transform);
            rainObject.transform.localPosition = new Vector3(0f, 18f, 0f);
            rainParticles = rainObject.AddComponent<ParticleSystem>();

            var main = rainParticles.main;
            main.startColor = new Color(0.5f, 0.6f, 0.8f, 0.65f);
            main.startSize = 0.12f;
            main.loop = true;
            main.maxParticles = 500;

            var emission = rainParticles.emission;
            emission.rateOverTime = 0f;

            var shape = rainParticles.shape;
            shape.shapeType = ParticleSystemShapeType.Box;
            shape.scale = new Vector3(80f, 1f, 80f);

            var velocity = rainParticles.velocityOverLifetime;
            velocity.enabled = true;
            velocity.space = ParticleSystemSimulationSpace.World;
            velocity.x = new ParticleSystem.MinMaxCurve(0f, 0f);
            velocity.y = new ParticleSystem.MinMaxCurve(-15f, -22f);
            velocity.z = new ParticleSystem.MinMaxCurve(0f, 0f);

            rainObject.SetActive(false);
        }
    }
}
