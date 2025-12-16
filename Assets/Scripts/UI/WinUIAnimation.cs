using System;
using GameCycle;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class WinUIAnimation : MonoCashed<Animation>
    {
        [SerializeField] private Button _nextButton;

        private AudioSource _audioSource;
        
        protected override void Awake()
        {
            base.Awake();

            Cashed1.enabled = false;
            Cashed1.playAutomatically = false;
            _audioSource = GetComponent<AudioSource>();
        }

        private void OnEnable()
        {
            GameController.OnFillProgress += GameControllerOnOnFillProgress;
        }

        private void OnDisable()
        {
            GameController.OnFillProgress -= GameControllerOnOnFillProgress;
            _nextButton.transform.localScale = Vector3.one;
        }

        private void GameControllerOnOnFillProgress()
        {
            Cashed1.enabled = true;
            Cashed1.Play();
            _audioSource.Play();
        }
    }
}
