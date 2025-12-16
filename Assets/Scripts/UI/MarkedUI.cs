using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class MarkedUI : MonoBehaviour
    {
        [SerializeField] protected Transform _markImage;
        [SerializeField] protected bool _isInvert;

        public bool IsCleared =>
            _isInvert ? !_markImage.gameObject.activeInHierarchy : _markImage.gameObject.activeInHierarchy;

        protected virtual void OnEnable() => ResetCleared();

        public virtual void SetCleared()
        {
            _markImage.gameObject.SetActive(!_isInvert);
        }

        protected virtual void ResetCleared()
        {
            _markImage.gameObject.SetActive(_isInvert);
        }
    }
}
