using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TwoHandGrabInteractable : XRGrabInteractable
{
    public List<XRSimpleInteractable> secondGrabPoints = new();
    XRBaseInteractor secondInteractor;

    public enum TwoHandRotationType
    {
        None,
        First,
        Second
    }
    public TwoHandRotationType twoHandRotationType;

    public bool snapToSecondHand = true;
    private Quaternion initialRotationOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (var point in secondGrabPoints)
        {
            point.selectEntered.AddListener(OnSecondHandGrab);
            point.selectExited.AddListener(OnSecondHandRelease);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (secondInteractor && interactorsSelecting.Count > 0) 
        {
            var primaryInteractor = interactorsSelecting.Count > 0
                ? interactorsSelecting[0] as XRBaseInteractor
                : null;

            if (primaryInteractor != null)
            {
                primaryInteractor.attachTransform.rotation = GetTwoHandRotation(primaryInteractor) * (snapToSecondHand ? initialRotationOffset : Quaternion.identity);
            }

        }
        base.ProcessInteractable(updatePhase);
    }

    private Quaternion GetTwoHandRotation( XRBaseInteractor primaryInteractor)
    {
        Quaternion targetRotation;

        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - primaryInteractor.attachTransform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - primaryInteractor.attachTransform.position, primaryInteractor.attachTransform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position - primaryInteractor.attachTransform.position, secondInteractor.attachTransform.up);
        }

        return targetRotation;
    }

    void OnSecondHandGrab(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        secondInteractor = interactor;

        var primaryInteractor = interactorsSelecting.Count > 0
                ? interactorsSelecting[0] as XRBaseInteractor
                : null;

        if (primaryInteractor != null)
        {
            initialRotationOffset = Quaternion.Inverse(GetTwoHandRotation(primaryInteractor)) * primaryInteractor.attachTransform.rotation;
        }
    }


    void OnSecondHandRelease(SelectExitEventArgs args)
    {
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        secondInteractor = null;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        base.OnSelectEntered(args);
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        XRBaseInteractor interactor = args.interactorObject as XRBaseInteractor;
        base.OnSelectExited(args);
        secondInteractor = null;
    }

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        bool isAlreadyGrabbed = interactorsSelecting.Count > 0 && !interactorsSelecting.Contains(interactor);
        return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
    }
}
