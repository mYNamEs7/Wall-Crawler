using System;
using GameCycle;
using Static;
using UnityEngine;
using Random = UnityEngine.Random;

namespace UI
{
    public class RaceInfoUI : MonoBehaviour
    {
        public static event Action OnRaceComplete;
        
        [SerializeField] private RectTransform[] _moveObjects;
        [SerializeField] private Transform[] _players;

        private void OnEnable()
        {
            GameController.Instance.OnMainMenuStateEnter += InstanceOnOnMainMenuStateEnter;

            if (StaticData.RaceCompletedLevels <= 5 || StaticData.IsRace == 0)
            {
                gameObject.SetActive(false);
                return;
            }
            
            if (StaticData.RaceCompletedLevels - 6 >= 15)
            {
                gameObject.SetActive(false);
                OnRaceComplete?.Invoke();
            }
            
            foreach (var player in _players)
            {
                player.gameObject.SetActive(false);
            }
            _players[Random.Range(0, _players.Length)].gameObject.SetActive(true);
            
            foreach (var obj in _moveObjects)
            {
                var currentPos = obj.anchoredPosition;
                currentPos.x = -99 + 28.5f * (StaticData.RaceCompletedLevels - 6);
                obj.anchoredPosition = currentPos;
            }
        }

        private void InstanceOnOnMainMenuStateEnter()
        {
            gameObject.SetActive(true);
        }
    }
}
