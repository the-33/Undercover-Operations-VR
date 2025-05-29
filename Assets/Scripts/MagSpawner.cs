using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MagSpawner : XRBaseInteractable
{
    [SerializeField]
    private GameObject grabbableObject;

    [SerializeField]
    private Transform transformToInstantiate;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        // Instantiate object
        GameObject newObject = Instantiate(grabbableObject, transformToInstantiate.position, Quaternion.identity);

        // Get grab interactable from prefab
        XRGrabInteractable objectInteractable = newObject.GetComponent<XRGrabInteractable>();

        // Select object into same interactor
        interactionManager.SelectEnter(args.interactorObject, objectInteractable);

        base.OnSelectEntered(args);
    }
}