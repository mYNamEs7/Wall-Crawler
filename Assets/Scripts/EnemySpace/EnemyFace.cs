using System;
using UnityEngine;

namespace EnemySpace
{
    public class EnemyFace : MonoCashed<Renderer>
    {
        [SerializeField] private Material _dieFace;

        public void Die() => Cashed1.material = _dieFace;
    }
}
