using System;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    public event Action<Impact> OnImpact;

    [SerializeField] private ImpactAgent[] impactAgents;

    private void Awake()
    {
        impactAgents = GetComponentsInChildren<ImpactAgent>();
    }


    private void OnEnable()
    {
        foreach (var agent in impactAgents)
        {
            agent.OnImpact += HandleImpact;
        }
    }

    private void OnDisable()
    {
        foreach (var agent in impactAgents)
        {
            agent.OnImpact -= HandleImpact;
        }
    }

    void HandleImpact(Impact data)
    {
        OnImpact?.Invoke(data);
    }

}


public class Impact
{
    public ImpactZone Zone { get; private set; }
    public ImpactZoneSide Side { get; private set; }

    public Impact(ImpactZone zone, ImpactZoneSide side)
    {
        Zone = zone;
        Side = side;
    }


    public enum ImpactZone
    {
        Head,
        Body,
        Arms,
        Legs
    }

    public enum ImpactZoneSide
    {
        Left,
        Right,
        None
    }
}
