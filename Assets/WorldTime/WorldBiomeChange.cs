using System;
using UnityEngine;

namespace WorldTime
{
    public class WorldBiomeChange : MonoBehaviour
    {
        [SerializeField] private WorldTime _worldTime;
        [SerializeField] private GameObject[] biomes;
        [SerializeField] public int daysPerBiome = 2;

        public event Action BossNight;
        public event Action NormalNight;

        private int currentBiomeIndex = 0;
        public int CurrentBiomeIndex => currentBiomeIndex;

        private void Start()
        {
            _worldTime.NightStarted += OnNightStarted;
            ActivateBiome(currentBiomeIndex);
        }

        private void OnDestroy()
        {
            _worldTime.NightStarted -= OnNightStarted;
        }

        private void OnNightStarted()
        {
            int day = _worldTime.CurrentDay;

            int dayInBiome = ((day - 1) % daysPerBiome) + 1;

            bool isLastDayOfBiome = dayInBiome == daysPerBiome;

            if (isLastDayOfBiome)
            {
                BossNight?.Invoke();
                SwitchToNextBiome();
            }
            else
            {
                NormalNight?.Invoke();
            }
        }

        private void SwitchToNextBiome()
        {
            biomes[currentBiomeIndex].SetActive(false);

            currentBiomeIndex++;
            currentBiomeIndex = Mathf.Clamp(currentBiomeIndex, 0, biomes.Length - 1);

            biomes[currentBiomeIndex].SetActive(true);
        }
        public void SetBiome(int index)
        {
            index = Mathf.Clamp(index, 0, biomes.Length - 1);
            ActivateBiome(index);
            currentBiomeIndex = index;
        }

        private void ActivateBiome(int index)
        {
            for (int i = 0; i < biomes.Length; i++)
                biomes[i].SetActive(i == index);
        }

    }
}
