using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YGLoader : MonoBehaviour
{
    [SerializeField] private Transform[] _objectsToEnable;
    
    private Transform _loadingScreen;

    private void Awake()
    {
        _loadingScreen = transform.GetChild(0);
    }

    private IEnumerator Start()
    {
        LevelManager.Instance.LevelChanged += InstanceOnLevelChanged;
        
        while (!YG.YandexGame.SDKEnabled) yield return null;

        YG.YandexGame.GameReadyAPI();
        
        yield return new WaitForSeconds(0.5f);

        yield return null;
        
        _loadingScreen.gameObject.SetActive(false);
        foreach (var obj in _objectsToEnable)
        {
            obj.gameObject.SetActive(true);
        }
    }

    private void InstanceOnLevelChanged(int obj)
    {
        StartCoroutine(LoadingRoutine());
    }

    public void Loading() => _loadingScreen.gameObject.SetActive(true);

    private IEnumerator LoadingRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        
        _loadingScreen.gameObject.SetActive(false);
    }
}
