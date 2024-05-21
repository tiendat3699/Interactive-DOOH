using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;

public class TextEffect : MonoBehaviour
{
    [SerializeField] private float _waitTime;
    [SerializeField] private float _waitEndTime;
    List<Animator> _animators;

    void Start()
    {
        _animators = new List<Animator>(GetComponentsInChildren<Animator>());

        StartCoroutine(DoAnimation());

    }

    IEnumerator DoAnimation(){
        while(true){
            foreach(var animator in _animators){
                animator.SetTrigger("DoAnimation");
                yield return new WaitForSeconds(_waitTime);
            }

            yield return new WaitForSeconds(_waitEndTime);
        }
    }
}
