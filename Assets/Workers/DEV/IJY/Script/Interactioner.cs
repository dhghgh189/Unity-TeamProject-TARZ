using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactioner : MonoBehaviour
{
    private Coroutine UpdateCoroutine;
    private LayerMask interactionLayer;
    [SerializeField] private bool IsInteraction_inside;
    [SerializeField] private PlayerController PlayerController;
    [Space(10f)]
    [SerializeField] private List<GameObject> interactionOBJs = new();

    private void Start()
    {
        IsInteraction_inside = false;
        interactionLayer = LayerMask.NameToLayer("Is_Interaction");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == interactionLayer)
        {
            IsInteraction_inside = true;
            UpdateCoroutine = StartCoroutine(OnEnableCheckRoutine(other.gameObject));
        }
    }

    IEnumerator OnEnableCheckRoutine(GameObject obj)
    {
        interactionOBJs.Add(obj);

        while (true)
        {
            if (obj == null || !obj.activeSelf)
            {
                IsInteraction_inside = false;
                yield break;
            }

            yield return null;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (UpdateCoroutine != null)
        {
            StopCoroutine(UpdateCoroutine);
            UpdateCoroutine = null;
        }

        interactionOBJs.Clear();
        IsInteraction_inside = false;
    }

    private void Update()
    {
        if (IsInteraction_inside)
        {
            if (PlayerController.PInput.TryInteraction)
            {
                SelectInteraction(interactionOBJs).Activate();
            }
        }
    }

    Base_InteractionOBJ SelectInteraction(List<GameObject> list)
    {
        foreach (GameObject obj in list)
        {
            Debug.Log($"정렬 전 : {obj}");
        }

        var target = from targeting in list
                     where targeting != null || targeting.gameObject.activeSelf
                     orderby Vector3.Distance(targeting.transform.position, transform.position) ascending
                     select targeting;
        list = target.ToList();

        foreach (GameObject obj in list)
        {
            Debug.Log($"<color=yellow>정렬 후 : {obj}</color>");
        }

        return list.First().GetComponent<Base_InteractionOBJ>();
    }
}
