using System;
using System.Collections;
using System.Linq;
using GameCycle;
using SO;
using Static;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private Text _fpsText;
    [SerializeField] private Text _currentLevelText;
    [SerializeField] private float _updateDelay = 0.1f;

    private float _count;
    private int frameCount = 0;
    private float deltaTime = 0.0f;
    private float _timer;
    private bool _isS;
    public static event Action OnDisableGameUI;

    public bool IsCleared => _isS;

    private IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            // Рассчитываем FPS
            _count = frameCount / deltaTime;
            _fpsText.text = string.Format("FPS: {0:F0}", _count);

            // Сбрасываем счетчики
            frameCount = 0;
            deltaTime = 0.0f;
        }
    }

    public void IncWinStreak() => StaticData.WinStreak++;

    public void NextLevel() => LevelManager.Instance.NextLevel();
    public void PrevLevel() => LevelManager.Instance.PrevLevel();

    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        _isS = true;
    }

    private void Update()
    {
        if (_isS)
        {
            Destroy(gameObject);
            return;
        }
        
        frameCount++;
        deltaTime += Time.deltaTime;
        
        var targetText = $"{LevelManager.Instance.CurrentLevelCount}";
        if (_currentLevelText.text != targetText)
            _currentLevelText.text = targetText;
    }
}