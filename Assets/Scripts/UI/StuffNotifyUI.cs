using System;
using System.Linq;
using PlayerSpace.Web;
using SO;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StuffNotifyUI : MonoBehaviour
    {
        [SerializeField] private Image _image;
        
        private Animation _visual;

        private static GameSettings.StuffOnLevel _currentStuff =>
            GameSettings.Instance.stuffOnLevel.FirstOrDefault(stuff =>
                stuff.level == LevelManager.Instance.CurrentLevelCount);
        
        private void OnEnable()
        {
            StuffUI.OnStuff += StuffUIOnOnStuff;
            
            _visual = transform.GetChild(0).GetComponent<Animation>();
            _visual.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            _visual.transform.localScale = Vector3.one;
            _visual.clip = _visual.GetClip("UI_QuestPopup_In_01");
            
            StuffUI.OnStuff -= StuffUIOnOnStuff;
        }

        private void StuffUIOnOnStuff()
        {
            _visual.gameObject.SetActive(true);
            _image.sprite = _currentStuff.image;
            _visual.Play();
        }

        public void InvokeOutAnimation()
        {
            _visual.clip = _visual.GetClip("UI_QuestPopup_Out_01");
            _visual.Play();
            WebAim.CanAim = true;
        }
    }
}
