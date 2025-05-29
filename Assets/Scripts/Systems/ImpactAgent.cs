using System;
using UnityEngine;

public class ImpactAgent : MonoBehaviour
{
    public event Action<Impact> OnImpact;
    [SerializeField] private Impact.ImpactZone zone;
    [SerializeField] private Impact.ImpactZoneSide side;

    [SerializeField] private LayerMask impactLayerMask;

    private Impact impact;

    private void Awake()
    {
        impact = new Impact(zone, side);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((impactLayerMask & (1 << collision.gameObject.layer)) == 0)
            return;

        OnImpact?.Invoke(impact);
    }
}
