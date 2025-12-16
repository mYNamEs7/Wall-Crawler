using System.Collections.Generic;
using EnemySpace;
using GameCycle;
using Static;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : Singleton<LevelManager>
{
    public enum IndexCheckMethod { None, Loop, Random }
    
    const string PREFS_KEY_LEVEL_ID = "CurrentLevelCount";
    const string PREFS_KEY_LAST_INDEX = "LastLevelIndex";

    public bool editorMode;
    public int CurrentLevelCount => PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID, 0) + 1;
    public event System.Action<int> LevelChanged;

    public int CurrentLevelIndex;

    public List<Level> Levels = new();

    public event System.Action OnLevelStarted;

    protected override void OnAwake()
    {
#if !UNITY_EDITOR
        editorMode = false;
#endif
        if (!editorMode) SelectLevel(PlayerPrefs.GetInt(PREFS_KEY_LAST_INDEX));
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt(PREFS_KEY_LAST_INDEX, CurrentLevelIndex);
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt(PREFS_KEY_LAST_INDEX, CurrentLevelIndex);
    }


    public void StartLevel()
    {
        //SendStart();
        OnLevelStarted?.Invoke();
    }

    public void RestartLevel()
    {
        //SendRestart();
        MyAnalytics.OnLevelRestart(CurrentLevelCount);
        GameController.ShowAd();
        RestartLevel(IndexCheckMethod.None);
    }

    public void RestartLevel(IndexCheckMethod indexCheck) => SelectLevel(CurrentLevelIndex, indexCheck);

    public void NextLevel() => NextLevel(IndexCheckMethod.Loop);

    public void NextLevel(IndexCheckMethod indexCheck)
    {
        //SendComplete();
        StaticData.CompletedLevels++;
        StaticData.RaceCompletedLevels++;
        MyAnalytics.OnLevelComplete(CurrentLevelCount);
        
        if (!editorMode)
            PlayerPrefs.SetInt(PREFS_KEY_LEVEL_ID, PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID) + 1);
        SelectLevel(CurrentLevelIndex + 1, indexCheck);
    }

    public void PrevLevel() => PrevLevel(IndexCheckMethod.Loop);

    public void PrevLevel(IndexCheckMethod indexCheck)
    {
        PlayerPrefs.SetInt(PREFS_KEY_LEVEL_ID, PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID) - 1);
        SelectLevel(CurrentLevelIndex - 1, indexCheck);
    }

    public void ClearListAtIndex(int levelIndex) => Levels[levelIndex].LevelPrefab = null;

    public void SelectLevel(int levelIndex, IndexCheckMethod indexCheck = IndexCheckMethod.Loop)
    {
        foreach (var player in FindObjectsOfType<Death>(true))
        {
            Destroy(player.gameObject);
        }
        
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        var isLoop = false;
        levelIndex = indexCheck switch
        {
            IndexCheckMethod.Loop => GetLoopIndex(levelIndex, out isLoop),
            IndexCheckMethod.Random => GetRandomIndex(levelIndex),
            _ => levelIndex
        };

        if (Levels[levelIndex].LevelPrefab == null)
        {
            Debug.Log("<color=red>There is no prefab attached!</color>");
            return;
        }

        var level = Levels[levelIndex];
        if (!level.LevelPrefab) return;
        
        SelLevelParams(level);
        CurrentLevelIndex = levelIndex;
        PlayerPrefs.SetInt(PREFS_KEY_LAST_INDEX, CurrentLevelIndex);
        
        LevelChanged?.Invoke(CurrentLevelIndex);
    }

    private int GetLoopIndex(int levelIndex, out bool isLoop)
    {
        if (levelIndex > Levels.Count - 1)
        {
            isLoop = true;
            return 5;
        }

        isLoop = false;
        if (levelIndex < 0) return Levels.Count - 1;
        return levelIndex;
    }

    private int GetRandomIndex(int levelIndex)
    {
        if (editorMode) return levelIndex > Levels.Count - 1 || levelIndex <= 0 ? 0 : levelIndex;
        
        var levelId = PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID);
        if (levelId <= Levels.Count - 1) return levelId;
        if (Levels.Count <= 1) return Random.Range(0, Levels.Count);

        while (true)
        {
            levelId = Random.Range(0, Levels.Count);
            if (levelId != CurrentLevelIndex) return levelId;
        }
    }

    private void SelLevelParams(Level level)
    {
        if (level.LevelPrefab)
        {
            ClearChilds();
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                Instantiate(level.LevelPrefab, transform);
            }
            else PrefabUtility.InstantiatePrefab(level.LevelPrefab, transform);
            foreach (var child in GetComponentsInChildren<IEditorModeSpawn>())
                child.EditorModeSpawn();
#else
            Instantiate(level.LevelPrefab, transform);
#endif
        }

        if (level.SkyboxMaterial)
        {
            RenderSettings.skybox = level.SkyboxMaterial;
        }
    }

    private void ClearChilds()
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            var destroyObject = transform.GetChild(i).gameObject;
            DestroyImmediate(destroyObject);
        }
    }



    #region Analitics Events

/*    public void SendStart()
    {
        string content = (PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID, 0) + 1).ToString();
    }

    public void SendRestart()
    {
        string content = (PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID, 0) + 1).ToString();
    }

    public void SendComplete()
    {
        string content = (PlayerPrefs.GetInt(PREFS_KEY_LEVEL_ID, 0) + 1).ToString();
    }*/

    #endregion
}

[System.Serializable]
public class Level
{
    public GameObject LevelPrefab;
    public Material SkyboxMaterial;
}