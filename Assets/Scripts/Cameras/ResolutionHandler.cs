using System;
using Static;
using UnityEngine;

namespace Cameras
{
    public class ResolutionHandler : MonoBehaviour
    {
        private Camera _camera;

        public static event Action OnChangeFov;

        public static float StartFOV { get; private set; }
        public static float ScaledFOV { get; private set; }

        private static float _pcFov = 45;

        public static float PC_FOV
        {
            get => _pcFov;
            set
            {
                _pcFov = value;
                OnChangeFov?.Invoke();
            }
        }

        private static float _baseFov = 60;

        public static float Base_FOV
        {
            get => _baseFov;
            set
            {
                _baseFov = value;
                OnChangeFov?.Invoke();
            }
        }

        private static float _maxFov = 75;

        public static float Max_FOV
        {
            get => _maxFov;
            set
            {
                _maxFov = value;
                OnChangeFov?.Invoke();
            }
        }

        private float screenRatio;

        private void Start()
        {
            _camera = Camera.main;
            SetFieldOfView();
        }

        private void OnEnable()
        {
            OnChangeFov += SetFieldOfView;
        }

        private void OnDisable()
        {
            OnChangeFov -= SetFieldOfView;
        }

        private void Update()
        {
            var currentRatio = Screen.height / (float)Screen.width;

            if (Mathf.Approximately(currentRatio, screenRatio)) return;
        
            screenRatio = currentRatio;
            SetFieldOfView();
        }

        private void SetFieldOfView()
        {
            print("change");
            screenRatio = (1.0f * Screen.height) / (1.0f * Screen.width);
            
            _camera.fieldOfView = screenRatio switch
            {
                < 1f => PC_FOV,
                > 1.7f and < 1.8f => Base_FOV,
                > 2.1f and < 2.2f => Max_FOV,
                _ => Base_FOV
            };

            StartFOV = _camera.fieldOfView;
            ScaledFOV = Mathf.Approximately(StartFOV, PC_FOV) ? PC_FOV - 10 : Base_FOV - 15;
        }

        public void ResetFOV() => _camera.fieldOfView = Base_FOV;
        public void FOVByResolution() => _camera.fieldOfView = StartFOV;
    }
}
