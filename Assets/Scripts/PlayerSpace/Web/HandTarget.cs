using System;
using UnityEngine;

namespace PlayerSpace.Web
{
    public class HandTarget : MonoBehaviour
    {
        private WebAim _webAim;

        private void Awake()
        {
            _webAim = FindObjectOfType<WebAim>();
        }

        private void LateUpdate()
        {
            var targetPosition = _webAim.DistantPoint;
            
            if (Mathf.Approximately(targetPosition.x, transform.position.x)) return;
            
            targetPosition.z = 0f;
            transform.position = targetPosition;
        }
    }
}
