using System;
using System.Collections;
using UnityEngine;

namespace WorldTime
{
    public class WorldTime : MonoBehaviour
    {
        public event EventHandler<TimeSpan> WorldTimeChanged;
        public event Action DayChanged;
        public event Action NightStarted;

        [SerializeField] private float _dayLength;
        [SerializeField, Range(0, 23)] private int startHour = 6;
        [SerializeField, Range(0, 59)] private int startMinute = 0;

        private TimeSpan _currentTime;
        private int _currentDay = 0;

        public int CurrentDay => _currentDay;

        private bool isPaused = false;

        public void SetDay(int day)
        {
            _currentDay = Mathf.Max(0, day);
        }

        private float _minuteLength => _dayLength / WorldTimeContstance.MinutesInDay;

        private void Start()
        {
            _currentTime = new TimeSpan(startHour, startMinute, 0);
            StartCoroutine(AddMinute());
        }

        private IEnumerator AddMinute()
        {
            while (true)
            {
                if (!isPaused)
                {
                    _currentTime += TimeSpan.FromMinutes(1);

                    if (_currentTime.TotalMinutes >= WorldTimeContstance.MinutesInDay)
                    {
                        _currentTime = TimeSpan.Zero;
                        _currentDay++;
                    }

                    if (_currentTime.Hours == WorldTimeContstance.DayStartHour &&
                        _currentTime.Minutes == 10)
                    {
                        DayChanged?.Invoke();
                    }

                    if (_currentTime.Hours == WorldTimeContstance.NightStartHour &&
                        _currentTime.Minutes == 0)
                    {
                        NightStarted?.Invoke();
                    }

                    WorldTimeChanged?.Invoke(this, _currentTime);
                }

                yield return new WaitForSeconds(_minuteLength);
            }
        }
        public void StopTime()
        {
            isPaused = true;
        }

        public void StartTime()
        {
            isPaused = false;
        }

        public void SetExactTime(int hour, int minute)
        {
            _currentTime = new TimeSpan(hour, minute, 0);
            WorldTimeChanged?.Invoke(this, _currentTime);
        }

    }
}
