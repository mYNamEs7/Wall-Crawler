using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnemySpace;
using Shop;
using UnityEngine;

namespace PlayerSpace.Web
{
    public class WebFire : MonoBehaviour
    {
        public static event Action OnWebDrawn;

        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private bool _isPreview;
        [SerializeField] private Transform _firstPoint;
        [SerializeField] private Transform _secondPoint;

        private Player _player;
        private bool _isPlayerFly;

        private LineRenderer Cashed1;

        protected void Awake()
        {
            _player = FindObjectOfType<Player>();
        }

        private void OnEnable()
        {
            Cashed1 = GetComponent<LineRenderer>();
            if (_isPreview)
            {
                ShowWeb();
                return;
            }
            
            WebAim.OnDrawWeb += WebAimOnOnDrawWeb;
            
            Player.OnStartFly += PlayerOnOnStartFly;
            Player.OnFinishFly += PlayerOnOnFinishFly;
            
            Stuff.OnStartFly += StuffOnOnStartFly;
            Stuff.OnFinishFly += StuffOnOnFinishFly;

            // ShopLoader.OnRopeSkinChanged += ShowWeb;

            
        }

        private void OnDisable()
        {
            if (_isPreview) return;
            
            WebAim.OnDrawWeb -= WebAimOnOnDrawWeb;
            
            Player.OnStartFly -= PlayerOnOnStartFly;
            Player.OnFinishFly -= PlayerOnOnFinishFly;
            
            Stuff.OnStartFly -= StuffOnOnStartFly;
            Stuff.OnFinishFly -= StuffOnOnFinishFly;
            
            // ShopLoader.OnRopeSkinChanged -= ShowWeb;
        }

        private void ShowWeb() => WebAimOnOnDrawWeb(_firstPoint.position, _secondPoint.position, new RaycastHit());
        
        private void StuffOnOnFinishFly()
        {
            Cashed1.positionCount = 0;
            WebAim.CanAim = true;
        }

        private void StuffOnOnStartFly(Transform target)
        {
            Cashed1.SetPosition(1, target.position);
        }

        private void PlayerOnOnFinishFly(Vector3 arg1, Vector3 arg2, Vector3 arg3, int arg4, Death arg5)
        {
            _isPlayerFly = false;
        }

        private void PlayerOnOnStartFly(Vector3 arg1, Vector3 arg2)
        {
            _isPlayerFly = true;
            StartCoroutine(WebErase());
        }

        private IEnumerator WebErase()
        {
            while (_isPlayerFly)
            {
                Cashed1.SetPosition(0, _player.CenterPoint);
                yield return null;
            }

            Cashed1.positionCount = 0;
        }

        public void FastWebErase() => Cashed1.positionCount = 0;

        private void WebAimOnOnDrawWeb(Vector3 startPosition, Vector3 finishPosition, RaycastHit hit)
        {
            StartCoroutine(DrawingWeb(startPosition, finishPosition, hit));
        }

        private IEnumerator DrawingWeb(Vector3 startPosition, Vector3 finishPosition, RaycastHit hit)
        {
            var startHit = Vector3.zero;
            
            if (hit.transform)
                startHit = hit.transform.position;
            
            Cashed1.positionCount = 2;
            Cashed1.SetPosition(0, _isPreview ? startPosition : _player.CenterPoint);
            Cashed1.SetPosition(1, _isPreview ? startPosition : _player.CenterPoint);

            var finishPos = startHit == Vector3.zero ? finishPosition : finishPosition + (hit.transform.position - startHit);
            while (Vector3.Distance(Cashed1.GetPosition(1), finishPos) > 0.7f)
            {
                var targetPos = startHit == Vector3.zero ? finishPosition : finishPosition + (hit.transform.position - startHit);
                
                var currentEndPoint = Vector3.Lerp(Cashed1.GetPosition(1), targetPos, _duration * Time.deltaTime);
                Cashed1.SetPosition(1, currentEndPoint);

                yield return null;
            }
            
            var finalPos = startHit == Vector3.zero ? finishPosition : finishPosition + (hit.transform.position - startHit);
            Cashed1.SetPosition(1, finalPos);
            if (!_isPreview)
                OnWebDrawn?.Invoke();
        }
    }
}
