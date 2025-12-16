using System;
using Cameras;
using SO;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerSpace.Web
{
    public class WebTarget : MonoBehaviour
    {
        [SerializeField] private WebTargetType _type;
        [SerializeField] private int _enemyLayer = 6;
        [SerializeField] private int _stuffLayer = 10;

        private Camera _camera;
        private Transform _image;
        private Animation _animation;
        private Vector3 _lastPosition;
        private Vector3 _lastCutPosition;

        private enum WebTargetType
        {
            Enemy,
            Wall,
            MaxDistance,
            Stuff,
            CutObstacle,
            RopeEnd
        }
        private WebAim _webAim;

        private void OnEnable()
        {
            _image = transform.GetChild(0);
            _animation = GetComponent<Animation>();
            if (!_animation)
                _animation = GetComponentInChildren<Animation>();
            _camera = Camera.main;
            _webAim = FindObjectOfType<WebAim>();
            _image.gameObject.SetActive(false);
            
            WebAim.OnWebVisibleChanged += WebAimOnOnWebVisibleChanged;
            WebAim.OnMaxDistance += WebAimOnOnMaxDistance;
            _webAim.OnHitLayerChanged += WebAimOnOnHitLayerChanged;
        }
        
        private void OnDisable()
        {
            WebAim.OnWebVisibleChanged -= WebAimOnOnWebVisibleChanged;
            WebAim.OnMaxDistance -= WebAimOnOnMaxDistance;
        }
        
        private void WebAimOnOnMaxDistance(bool isMaxDistance)
        {
            if (_type == WebTargetType.MaxDistance)
                _image.gameObject.SetActive(isMaxDistance);
            else if(isMaxDistance)
                _image.gameObject.SetActive(false);
        }

        private void WebAimOnOnHitLayerChanged(int layer)
        {
            if(!_image.gameObject.activeInHierarchy && _animation)
                _animation.Play();

            if (_type != WebTargetType.CutObstacle)
            {
                if (layer == _enemyLayer)
                    _image.gameObject.SetActive(_type == WebTargetType.Enemy);
                else if (layer == _stuffLayer)
                    _image.gameObject.SetActive(_type == WebTargetType.Stuff);
                else
                    _image.gameObject.SetActive(_type == WebTargetType.Wall);
            }

            if (_type != WebTargetType.CutObstacle) return;
            
            if (layer == 11 && !_image.gameObject.activeInHierarchy)
            {
                _image.gameObject.SetActive(true);
                _animation.Play();
            }
            else if (layer != 11 && _image.gameObject.activeInHierarchy)
                _image.gameObject.SetActive(false);

        }

        private void WebAimOnOnWebVisibleChanged(bool isEnabled)
        {
            _image.gameObject.SetActive(_type == WebTargetType.RopeEnd ? !isEnabled : isEnabled);
        }

        private void FixedUpdate()
        {
            if (_lastPosition == _webAim.LastPoint) return;
            
            _lastPosition = _webAim.LastPoint;
            var screenPoint = _camera.WorldToScreenPoint(_lastPosition);

            transform.position = _type == WebTargetType.RopeEnd ? _lastPosition : screenPoint;
            
            SetCutPosition();
        }

        private void SetCutPosition()
        {
            if (!_webAim.IsCutObstacle || _type != WebTargetType.CutObstacle) return;
            
            var lastPos = _webAim.CutHit.point;
            
            if (_lastCutPosition == lastPos) return;
            
            _lastCutPosition = lastPos;
            var screenPoint = _camera.WorldToScreenPoint(_lastCutPosition);
            
            transform.position = screenPoint;
        }
    }
}
