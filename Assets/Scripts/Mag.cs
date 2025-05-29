using UnityEngine;

public class Mag : MonoBehaviour
{
    public int BulletsLeft;
    [SerializeField]private int MaxBullets;

    private void Start()
    {
        BulletsLeft = MaxBullets;
    }
}
