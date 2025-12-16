using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCycle;
using SO;
using Static;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public class TaskGridUI : MonoBehaviour
    {
        [SerializeField] private Tasks[] texts;
        [SerializeField] private Text _timeText;
        
        private ItemTaskUI[] itemTasks;
        private DateTime lastUpdateTime;
        private readonly TimeSpan interval = TimeSpan.FromHours(24);
        private readonly List<int> lastTextIndices = new();

        private void Awake()
        {
            if (LevelManager.Instance.CurrentLevelCount < 6)
            {
                transform.parent.parent.parent.gameObject.SetActive(false);
                return;
            }
            
            itemTasks = GetComponentsInChildren<ItemTaskUI>();
            
            LoadLastUpdateTime();
            LoadLastTextIndices();

            if (IsTimeToUpdate())
            {
                UpdateTexts();
            }
            else
            {
                for (var i = 0; i < itemTasks.Length; i++)
                {
                    SetTextByIndex(i, lastTextIndices[i]);
                }
            }
            
            transform.parent.parent.parent.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            itemTasks = GetComponentsInChildren<ItemTaskUI>();
            
            LoadLastUpdateTime();
            LoadLastTextIndices();

            if (IsTimeToUpdate())
            {
                UpdateTexts();
            }
            else
            {
                for (var i = 0; i < itemTasks.Length; i++)
                {
                    SetTextByIndex(i, lastTextIndices[i]);
                }
            }

            StartCoroutine(UpdateTimer());
        }

        private void OnDisable()
        {
            foreach (var itemTask in itemTasks)
            {
                itemTask.SetText("SOUND");
            }
        }

        private void SetTextByIndex(int i, int textIndex)
        {
            var targetText = GameSettings.Instance.CurrentLanguageIndex == 0
                ? texts[textIndex].ruText
                : texts[textIndex].enText;
            
            itemTasks[i].SetText(targetText);

            var similarTextCount = itemTasks.Count(task => task.GetText() == targetText);
            itemTasks[i].SetCount(texts[textIndex].GetKey(), texts[textIndex].count * similarTextCount);
            
            StaticData.SetTask(i, texts[textIndex].GetKey());
            StaticData.SetTaskCount(i, texts[textIndex].count * similarTextCount);
        }

        private void LoadLastUpdateTime()
        {
            if (PlayerPrefs.HasKey("LastUpdateTime"))
            {
                var lastUpdateTimeString = PlayerPrefs.GetString("LastUpdateTime");
                lastUpdateTime = DateTime.Parse(lastUpdateTimeString);
            }
            else
            {
                lastUpdateTime = DateTime.Now;
                SaveLastUpdateTime();
            }
        }

        private void SaveLastUpdateTime()
        {
            PlayerPrefs.SetString("LastUpdateTime", lastUpdateTime.ToString());
            PlayerPrefs.Save();
        }

        private void LoadLastTextIndices()
        {
            lastTextIndices.Clear();
            for (var i = 0; i < itemTasks.Length; i++)
            {
                if (StaticData.GetLastTextIndex(i) != -1)
                    lastTextIndices.Add(StaticData.GetLastTextIndex(i));
                else
                {
                    lastTextIndices.Add(UnityEngine.Random.Range(0, texts.Length));
                    StaticData.SetLastTextIndex(i, lastTextIndices[i]);
                }
            }
        }

        private bool IsTimeToUpdate()
        {
            return DateTime.Now - lastUpdateTime >= interval;
        }

        private void UpdateTexts()
        {
            lastTextIndices.Clear();
            for (var i = 0; i < itemTasks.Length; i++)
            {
                var randomIndex = UnityEngine.Random.Range(0, texts.Length);

                lastTextIndices.Add(randomIndex);
                SetTextByIndex(i, randomIndex);
                StaticData.SetLastTextIndex(i, randomIndex);
            }

            lastUpdateTime = DateTime.Now;
            SaveLastUpdateTime();

            for (var i = 0; i < 3; i++)
            {
                StaticData.SetClaimedRewardIndex(i, 0);
            }

            for (var i = 0; i < 5; i++)
            {
                StaticData.SetClaimedTaskIndex(i, 0);
                StaticData.SetSkippedTaskIndex(i, -1);
            }

            StaticData.KilledEnemies = 0;
            StaticData.CompletedLevels = 0;
            StaticData.OnWall = 0;
            StaticData.FoundedItems = 0;
            StaticData.AdWatchedCount = 0;
            StaticData.RaceCompletedLevels = 0;
        }

        private System.Collections.IEnumerator UpdateTimer()
        {
            while (true)
            {
                var timeUntilNextUpdate = (lastUpdateTime + interval) - DateTime.Now;
                if (timeUntilNextUpdate <= TimeSpan.Zero)
                {
                    UpdateTexts();
                    timeUntilNextUpdate = interval;
                }

                var h = GameSettings.Instance.CurrentLanguageIndex == 0 ? "ч" : "h";
                var m = GameSettings.Instance.CurrentLanguageIndex == 0 ? "м" : "m";
                _timeText.text = $"{(int)timeUntilNextUpdate.TotalHours:D2}{h}:{timeUntilNextUpdate.Minutes:D2}{m}";
                yield return new WaitForSeconds(1);
            }
        }
        
        [Serializable]
        private struct Tasks
        {
            public int id;
            public string ruText;
            public string enText;
            public int count;

            public int GetKey() => id switch
            {
                0 => StaticData.KilledEnemies,
                1 => StaticData.CompletedLevels,
                2 => StaticData.OnWall,
                3 => StaticData.FoundedItems,
                4 => StaticData.AdWatchedCount,
                _ => 0
            };
        }
    }
}
