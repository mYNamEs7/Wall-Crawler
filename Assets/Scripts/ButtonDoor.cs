using System;
using System.Collections;
using System.Collections.Generic;
using EnemySpace;
using PlayerSpace;
using PlayerSpace.Web;
using SO;
using UnityEngine;

public class ButtonDoor : MonoCashed<Animation>
{
    private enum Type
    {
        Button,
        Door
    }
    
    private enum Color
    {
        Color1,
        Color2,
        Color3
    }

    private static event Action<Color> OnButtonPush;

    [SerializeField] private Type _type;
    [SerializeField] private Color _color;
    [SerializeField] private List<MeshRenderer> _buttonMeshs;

    [Header("Button Materials")] 
    [SerializeField] private Material _blue;
    [SerializeField] private Material _red;
    [SerializeField] private Material _yellow;

    private int layer;

    private void OnEnable()
    {
        ButtonDoor.OnButtonPush += ButtonDoorOnOnButtonPush;
        WebAim.OnPlayerFly += WebAimOnOnPlayerFly;
    }

    private void OnDisable()
    {
        ButtonDoor.OnButtonPush -= ButtonDoorOnOnButtonPush;
        WebAim.OnPlayerFly -= WebAimOnOnPlayerFly;
    }

    private void WebAimOnOnPlayerFly(Vector3 arg1, Vector3 arg2, Vector3 arg3, int arg4, Death arg5, Transform arg6, RaycastHit arg7)
    {
        layer = arg4;
        print($"LAYYYYER {layer}");
    }

    private void OnValidate()
    {
        if (_buttonMeshs.Count <= 0 || !_blue || !_red || !_yellow) return;

        _buttonMeshs.ForEach(mesh => mesh.material = _color switch
        {
            Color.Color1 => _blue,
            Color.Color2 => _red,
            Color.Color3 => _yellow,
            _ => throw new ArgumentOutOfRangeException()
        });
    }

    private void ButtonDoorOnOnButtonPush(Color color)
    {
        if (_type == Type.Door && _color == color)
        {
            var laser = GetComponentInChildren<Laser>();
            if(laser)
                laser.Deactivate();
            else
            {
                foreach (var mesh in _buttonMeshs)
                {
                    mesh.gameObject.layer = 13;
                }
                
                Cashed1.Play();
                Destroy(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (layer != 14 && LevelManager.Instance.CurrentLevelIndex == 22) return;
        
        print(GameSettings.Instance.layers.playerMask.value);
        if (_type == Type.Button)
        {
            if ((GameSettings.Instance.layers.playerMask & (1 << other.gameObject.layer)) != 0 || (GameSettings.Instance.layers.obstacleMask & (1 << other.gameObject.layer)) != 0)
            {
                
                Cashed1.clip = Cashed1.GetClip("Button_Push_01");
                Cashed1.Play();

                OnButtonPush?.Invoke(_color);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (layer != 14 && LevelManager.Instance.CurrentLevelIndex == 22) return;
        
        if (_type == Type.Button)
        {
            if ((GameSettings.Instance.layers.playerMask & (1 << other.gameObject.layer)) != 0 || (GameSettings.Instance.layers.obstacleMask & (1 << other.gameObject.layer)) != 0)
            {
                Cashed1.clip = Cashed1.GetClip("Button_UnPush_01");
                Cashed1.Play();
            }
        }
    }
}
