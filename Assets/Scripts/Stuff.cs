using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlayerSpace.Web;
using Static;
using UnityEditor;
using UnityEngine;

public class Stuff : MonoBehaviour
{
    public static event Action<Transform> OnStartFly;
    public static event Action OnFinishFly;

    [SerializeField] private int _stuffIndex;

    private void OnEnable()
    {
        WebAim.OnGetStuff += WebAimOnOnGetStuff;
    }

    private void OnDisable()
    {
        WebAim.OnGetStuff -= WebAimOnOnGetStuff;
    }

    private void WebAimOnOnGetStuff(Transform target)
    {
        StartCoroutine(GetStuff(target));
    }

    private IEnumerator GetStuff(Transform target)
    {
        while (Vector3.Distance(transform.position, target.position) > 0.3f)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, 5 * Time.deltaTime);
            OnStartFly?.Invoke(transform);
            yield return null;
        }

        transform.position = target.position;
        OnFinishFly?.Invoke();
        
        StaticData.Stuff.SetStuffByIndex(_stuffIndex, 1);
        StaticData.FoundedItems++;
        
        gameObject.SetActive(false);
    }
}
