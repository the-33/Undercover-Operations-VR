using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class HideHandOnGrab : MonoBehaviour
{
    public XRBaseInteractor interactor;  // Aquí asignarás tu Near-Far Interactor
    public GameObject handModel;         // El modelo visual de la mano

    private void OnEnable()
    {
        interactor.selectEntered.AddListener(OnGrab);
        interactor.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        interactor.selectEntered.RemoveListener(OnGrab);
        interactor.selectExited.RemoveListener(OnRelease);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        handModel.SetActive(false);
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        handModel.SetActive(true);
    }
}
