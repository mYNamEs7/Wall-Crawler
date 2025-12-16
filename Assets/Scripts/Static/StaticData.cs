using UnityEngine;

namespace Static
{
    public static class StaticData
    {
        public static int RewardProgressStep;
        public static int ProgressLevel;

        public static Stuff Stuff { get; } = new();
        
        private const string KEY_CLAIMED_ITEMS = "ClaimedItemsKey";
        public static string ClaimedItems
        {
            get => PlayerPrefs.GetString(KEY_CLAIMED_ITEMS, "");
            set
            {
                PlayerPrefs.SetString(KEY_CLAIMED_ITEMS, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_WIN_STREAK = "WinStreakKey";
        public static int WinStreak
        {
            get => PlayerPrefs.GetInt(KEY_WIN_STREAK, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_WIN_STREAK, value);
                PlayerPrefs.Save();
            }
        }

        private const string KEY_REWARD_PROGRESS = "RewardProgressKey";
        public static int RewardProgress
        {
            get => PlayerPrefs.GetInt(KEY_REWARD_PROGRESS, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_REWARD_PROGRESS, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_MONEY_AMOUNT = "MoneyAmountKey";
        public static int MoneyAmount
        {
            get => PlayerPrefs.GetInt(KEY_MONEY_AMOUNT, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_MONEY_AMOUNT, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_SELECTED_CHARACTER_ID = "SelectedCharacterIdKey";
        public static int SelectedCharacterId
        {
            get => PlayerPrefs.GetInt(KEY_SELECTED_CHARACTER_ID, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_SELECTED_CHARACTER_ID, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_SELECTED_ROPE_ID = "SelectedRopeIdKey";
        public static int SelectedRopeId
        {
            get => PlayerPrefs.GetInt(KEY_SELECTED_ROPE_ID, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_SELECTED_ROPE_ID, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_PREVIEW_SELECTED_CHARACTER_ID = "PreviewSelectedCharacterIdKey";
        public static int PreviewSelectedCharacterId
        {
            get => PlayerPrefs.GetInt(KEY_PREVIEW_SELECTED_CHARACTER_ID, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_PREVIEW_SELECTED_CHARACTER_ID, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_PREVIEW_SELECTED_ROPE_ID = "PreviewSelectedRopeIdKey";
        public static int PreviewSelectedRopeId
        {
            get => PlayerPrefs.GetInt(KEY_PREVIEW_SELECTED_ROPE_ID, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_PREVIEW_SELECTED_ROPE_ID, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_CLAIMED_CHARACTER_ID = "ClaimedCharacterIdKey";
        public static int ClaimedCharacterId
        {
            get => PlayerPrefs.GetInt(KEY_CLAIMED_CHARACTER_ID, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_CLAIMED_CHARACTER_ID, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_CLAIMED_ROPE_ID = "ClaimedRopeIdKey";
        public static int ClaimedRopeId
        {
            get => PlayerPrefs.GetInt(KEY_CLAIMED_ROPE_ID, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_CLAIMED_ROPE_ID, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_IS_WEB_STREAK_REWARD = "IsWebStreakRewardKey";
        public static int IsWebStreakReward
        {
            get => PlayerPrefs.GetInt(KEY_IS_WEB_STREAK_REWARD, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_IS_WEB_STREAK_REWARD, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_LAST_TEXT_INDEX = "LastTextIndex";
        public static int GetLastTextIndex(int i) => PlayerPrefs.GetInt(KEY_LAST_TEXT_INDEX + i, -1);

        public static void SetLastTextIndex(int i, int value)
        {
            PlayerPrefs.SetInt(KEY_LAST_TEXT_INDEX + i, value);
            PlayerPrefs.Save();
        }
        
        private const string KEY_CLAIMED_TASK_INDEX = "ClaimedTaskIndex";
        public static int GetClaimedTaskIndex(int i) => PlayerPrefs.GetInt(KEY_CLAIMED_TASK_INDEX + i, 0);

        public static void SetClaimedTaskIndex(int i, int value)
        {
            PlayerPrefs.SetInt(KEY_CLAIMED_TASK_INDEX + i, value);
            PlayerPrefs.Save();
        }
        
        private const string KEY_SKIPPED_TASK_INDEX = "SkippedTaskIndex";
        public static int GetSkippedTaskIndex(int i) => PlayerPrefs.GetInt(KEY_SKIPPED_TASK_INDEX + i, -1);

        public static void SetSkippedTaskIndex(int i, int value)
        {
            PlayerPrefs.SetInt(KEY_SKIPPED_TASK_INDEX + i, value);
            PlayerPrefs.Save();
        }
        
        private const string KEY_CLAIMED_REWARD_INDEX = "ClaimedRewardIndex";
        public static int GetClaimedRewardIndex(int i) => PlayerPrefs.GetInt(KEY_CLAIMED_REWARD_INDEX + i, 0);

        public static void SetClaimedRewardIndex(int i, int value)
        {
            PlayerPrefs.SetInt(KEY_CLAIMED_REWARD_INDEX + i, value);
            PlayerPrefs.Save();
        }
        
        private const string KEY_KILLED_ENEMIES = "KilledEnemiesKey";
        public static int KilledEnemies
        {
            get => PlayerPrefs.GetInt(KEY_KILLED_ENEMIES, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_KILLED_ENEMIES, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_COMPLETED_LEVELS = "CompletedLevelsKey";
        public static int CompletedLevels
        {
            get => PlayerPrefs.GetInt(KEY_COMPLETED_LEVELS, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_COMPLETED_LEVELS, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_ON_WALL = "OnWallKey";
        public static int OnWall
        {
            get => PlayerPrefs.GetInt(KEY_ON_WALL, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_ON_WALL, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_FOUNDED_ITEMS = "FoundedItemsKey";
        public static int FoundedItems
        {
            get => PlayerPrefs.GetInt(KEY_FOUNDED_ITEMS, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_FOUNDED_ITEMS, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_AD_WATCHED_COUNT = "AdWatchedCountKey";
        public static int AdWatchedCount
        {
            get => PlayerPrefs.GetInt(KEY_AD_WATCHED_COUNT, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_AD_WATCHED_COUNT, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_IS_RACE = "IsRaceKey";
        public static int IsRace
        {
            get => PlayerPrefs.GetInt(KEY_IS_RACE, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_IS_RACE, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_RACE_COMPLETED_LEVELS = "RaceCompletedLevelsKey";
        public static int RaceCompletedLevels
        {
            get => PlayerPrefs.GetInt(KEY_RACE_COMPLETED_LEVELS, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_RACE_COMPLETED_LEVELS, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_IS_SOUND = "IsSoundKey";
        public static int IsSound
        {
            get => PlayerPrefs.GetInt(KEY_IS_SOUND, 1);
            set
            {
                PlayerPrefs.SetInt(KEY_IS_SOUND, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_FIRST_REWARD_STATE = "FirstRewardStateKey";
        public static int FirstRewardState
        {
            get => PlayerPrefs.GetInt(KEY_FIRST_REWARD_STATE, 0);
            set
            {
                PlayerPrefs.SetInt(KEY_FIRST_REWARD_STATE, value);
                PlayerPrefs.Save();
            }
        }
        
        private const string KEY_TASK = "TaskKey";
        public static int GetTask(int i) => PlayerPrefs.GetInt(KEY_TASK + i, -1);

        public static void SetTask(int i, int value)
        {
            PlayerPrefs.SetInt(KEY_TASK + i, value);
            PlayerPrefs.Save();
        }
        
        private const string KEY_TASK_COUNT = "TaskCountKey";
        public static int GetTaskCount(int i) => PlayerPrefs.GetInt(KEY_TASK_COUNT + i, -1);

        public static void SetTaskCount(int i, int value)
        {
            PlayerPrefs.SetInt(KEY_TASK_COUNT + i, value);
            PlayerPrefs.Save();
        }
    }
}
