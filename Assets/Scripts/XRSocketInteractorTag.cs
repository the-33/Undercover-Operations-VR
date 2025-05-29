using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class XRSocketInteractorTag : XRSocketInteractor
{
    public string targetTag;
    public GameObject currentAttachedObject = null;

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.CompareTag(targetTag);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        currentAttachedObject = args.interactableObject.transform.gameObject;
        print(currentAttachedObject.name);
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        currentAttachedObject = null;
        base.OnSelectExited(args);
    }
}
