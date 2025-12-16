using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayerSpace.Web;
using SO;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StuffUI : MonoCashed<Animation>
    {
        public static event Action OnStuff;

        [SerializeField] private Image _image;

        private static IEnumerable<int> _stuffLevels => GameSettings.Instance.stuffOnLevel.Select(stuff => stuff.level);

        private static GameSettings.StuffOnLevel _currentStuff =>
            GameSettings.Instance.stuffOnLevel.FirstOrDefault(stuff =>
                stuff.level == LevelManager.Instance.CurrentLevelCount);

        private void OnEnable()
        {
            if(!_stuffLevels.Contains(LevelManager.Instance.CurrentLevelCount))
                gameObject.SetActive(false);
            else
            {
                WebAim.CanAim = false;
                _image.sprite = _currentStuff.image;
                Cashed1.Play();
                StartCoroutine(WaitForInvokeNotify());
            }
        }

        private static IEnumerator WaitForInvokeNotify()
        {
            yield return new WaitForSeconds(1f);
            
            OnStuff?.Invoke();
        }

        private void OnDisable()
        {
            gameObject.SetActive(true);
        }
    }
}
