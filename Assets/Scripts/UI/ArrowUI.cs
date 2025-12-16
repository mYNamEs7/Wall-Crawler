using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ArrowUI : MonoBehaviour
    {
        public static event Action OnStreakCompleted;
        public static event Action OnResetTimer;
        
        [SerializeField] private List<float> _rotationStates;
        [SerializeField] private Transform _nextStreakLevel;
        [SerializeField] private int _maxStreak;

        private void Awake()
        {
            if (StaticData.WinStreak - 1 > 21)
            {
                StaticData.WinStreak = 2;
                OnResetTimer?.Invoke();
                _nextStreakLevel.gameObject.SetActive(true);
                transform.parent.parent.gameObject.SetActive(false);
                return;
            }
            
            if (StaticData.WinStreak - 1 > _maxStreak)
            {
                // StaticData.WinStreak = 2;
                _nextStreakLevel.gameObject.SetActive(true);
                transform.parent.parent.gameObject.SetActive(false);
                return;
            }
            
            transform.eulerAngles = Vector3.forward * _rotationStates[GetRotationStateIndex()];
        }

        private void OnEnable()
        {
            if (StaticData.WinStreak - 1 > 21)
            {
                StaticData.WinStreak = 2;
                OnResetTimer?.Invoke();
                _nextStreakLevel.gameObject.SetActive(true);
                transform.parent.parent.gameObject.SetActive(false);
                return;
            }
            
            if (StaticData.WinStreak - 1 > _maxStreak)
            {
                // StaticData.WinStreak = 2;
                _nextStreakLevel.gameObject.SetActive(true);
                transform.parent.parent.gameObject.SetActive(false);
                return;
            }
            
            StartCoroutine(Rotate());
            // RotateArrow(transform.eulerAngles, Vector3.forward * GetRotationState(), 0.5f);
        }

        private IEnumerator Rotate()
        {
            yield return new WaitForSeconds(0.1f);

            RotateArrow(
                Vector3.forward *
                _rotationStates
                    [GetRotationStateIndex() - 1 < 0 ? GetRotationStateIndex() : GetRotationStateIndex() - 1],
                Vector3.forward * _rotationStates[GetRotationStateIndex()], 0.5f);
            // transform.eulerAngles = Vector3.forward * _rotationStates[GetRotationStateIndex()];
        }
        
        private void RotateArrow(Vector3 startValue, Vector3 targetValue, float duration)
        {
            StartCoroutine(ArrowRotation(startValue, targetValue, duration));
        }

        private IEnumerator ArrowRotation(Vector3 startValue, Vector3 targetValue, float duration)
        {
            var timer = 0f;
        
            while (timer < duration)
            {
                var progress = timer / duration;

                var currentProgress = Vector3.Lerp(startValue, targetValue, progress);
                transform.eulerAngles = currentProgress;
            
                timer += Time.deltaTime;

                yield return null;
            }

            transform.eulerAngles = targetValue;
        }

        private int GetRotationStateIndex()
        {
            if (StaticData.WinStreak - 1 >= _maxStreak)
                OnStreakCompleted?.Invoke();

            return StaticData.WinStreak - (_maxStreak - 7) - 2;
        }
    }
}
