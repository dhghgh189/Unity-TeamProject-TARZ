using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Interactioner : MonoBehaviour
{
    private LayerMask interactionLayer;
    [SerializeField] private bool IsInteraction_inside;
    [SerializeField] private PlayerController PlayerController;
    [Space(10f)]
    [SerializeField] private List<GameObject> interactionOBJs = new();
    //[SerializeField] private GameObject t;

    private void Start()
    {
        IsInteraction_inside = false;
        interactionLayer = LayerMask.NameToLayer("Is_Interaction");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == interactionLayer)
        {
            interactionOBJs.Add(other.gameObject);
            IsInteraction_inside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactionOBJs.Clear();
        IsInteraction_inside = false;
    }

    // 1. 키를 눌렀을 때, 오버랩 스피어를 통해 가장 거리가 가까운 오브젝트 하나를 골라 Activate
    // 2. 변수 하나에 인식한 오브젝트를 저장하고, 다시 다른 오브젝트가 닿았을 때 해당 오브젝트와 비교 하여 다시 변수를 설정

    private void Update()
    {
        if (IsInteraction_inside)
        {
            if (IsAlive() == false)
            {
                IsInteraction_inside = false;
                return;
            }

            if (PlayerController.PInput.TryInteraction)
            {
                Base_InteractionOBJ target = SelectInteraction();

                if (target == null)
                {
                    IsInteraction_inside = false;
                    return;
                }

                target.Activate();
            }
        }
    }

    bool IsAlive()
    {
        bool isAlive = true;

        for (int i = interactionOBJs.Count - 1; i >= 0; i--)
        {
            if (interactionOBJs[i] == null || !interactionOBJs[i].gameObject.activeSelf)
            {
                interactionOBJs.Remove(interactionOBJs[i]);
            }
        }
        if (interactionOBJs.Count > 0) isAlive = true;
        else isAlive = false;

        return isAlive;
    }

    Base_InteractionOBJ SelectInteraction()
    {
        var target = from targeting in interactionOBJs
                     orderby Vector3.Distance(targeting.transform.position, transform.position) ascending
                     select targeting;
        interactionOBJs = target.ToList();

        foreach (GameObject obj in interactionOBJs)
        {
            Debug.Log($"<color=yellow>정렬 후 : {obj}</color>");
        }

        if (interactionOBJs.Count <= 0) return null;
        return interactionOBJs.First().GetComponent<Base_InteractionOBJ>();
    }
}
