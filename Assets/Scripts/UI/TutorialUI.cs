using System;
using GameCycle;
using PlayerSpace.Web;
using UnityEngine;

namespace UI
{
    public class TutorialUI : MonoBehaviour
    {
        private void OnEnable()
        {
            gameObject.SetActive(LevelManager.Instance.CurrentLevelCount == 1);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) gameObject.SetActive(false);
        }
    }
}
