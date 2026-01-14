using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace WorldTime
{
    [RequireComponent (typeof(Light2D))]

    public class WorldLight : MonoBehaviour
    {
        private Light2D _light;

        [SerializeField] WorldTime _worldTime;
        [SerializeField] Gradient _gradient;

        private void Awake()
        {
            _light = GetComponent<Light2D>();
            _worldTime.WorldTimeChanged += OnWorldTimeChanged;
        }

        private void OnDestroy()
        {
            _worldTime.WorldTimeChanged -= OnWorldTimeChanged;
        }

        private void OnWorldTimeChanged(object sender, TimeSpan newTime)
        {
            _light.color = _gradient.Evaluate(PercentOfDay(newTime));
        }

        private float PercentOfDay(TimeSpan timeSpan)
        {
            return (float)timeSpan.TotalMinutes / WorldTimeContstance.MinutesInDay;
        }

    }

}
