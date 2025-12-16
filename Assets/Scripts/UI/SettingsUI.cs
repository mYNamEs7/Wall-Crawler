using System;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField] private Button _onButton;
        [SerializeField] private Button _offButton;
        
        private void Start()
        {
            DisableSounds();
            
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            UpdateButtons(true);
        }

        private void DisableSounds()
        {
            if (StaticData.IsSound == 0)
                AudioListener.volume = 0f;
        }

        private void UpdateButtons(bool isInvert)
        {
            _onButton.gameObject.SetActive(StaticData.IsSound == (isInvert ? 1 : 0));
            _offButton.gameObject.SetActive(StaticData.IsSound == (isInvert ? 0 : 1));
        }

        public void EnableSound()
        {
            StaticData.IsSound = 1;
            
            AudioListener.volume = 1f;
            
            UpdateButtons(true);
        }
        
        public void DisableSound()
        {
            StaticData.IsSound = 0;
            
            AudioListener.volume = 0f;
            
            UpdateButtons(true);
        }
    }
}
