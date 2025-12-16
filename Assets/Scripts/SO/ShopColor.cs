using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shop Color", menuName = "Static Data/Shop Color")]
public class ShopColor : ScriptableObject
{
    [SerializeField] private string _name;

    public bool CheckName(string colorName) => _name == colorName;
}
