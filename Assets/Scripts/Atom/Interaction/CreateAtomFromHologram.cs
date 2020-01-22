using System.Collections;
using System.Collections.Generic;
using Atom.Builder;
using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Input;
using Player;
using UnityEngine;

public class CreateAtomFromHologram : MonoBehaviour, IMixedRealityInputHandler
{
    [Header("Input")]
    [SerializeField] private MixedRealityInputAction action = MixedRealityInputAction.None;

    [SerializeField] private AtomBuilder atomBuilder = null;
    
    private GrabbableController grabbableControllerInTrigger = null;

    private void Awake()
    {
        CoreServices.InputSystem?.RegisterHandler<IMixedRealityInputHandler>(this);
    }

    public void OnInputDown(InputEventData eventData)
    {
        if (eventData.MixedRealityInputAction == action && grabbableControllerInTrigger)
        {
            atomBuilder.CreateAtom(grabbableControllerInTrigger.transform.position);
        }
    }

    public void OnInputUp(InputEventData eventData) { }

    private void OnTriggerEnter(Collider other)
    {
        GrabbableController grabbableController = other.GetComponent<GrabbableController>();
        if (grabbableController) grabbableControllerInTrigger = grabbableController;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<GrabbableController>()) grabbableControllerInTrigger = null;
    }
}
