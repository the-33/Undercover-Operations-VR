using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class Gun : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform shootingPoint;
    public float shootingForce;
    public int maxBulletsInScene;

    public GameObject shellPrefab;
    public Transform shellEjectionPoint;
    public float shellEjectionForce;

    public List<GameObject> bulletPool = new();

    public ParticleSystem smokeParticles;

    private Mag currentMag = null;

    public Animator animator;

    public XRSocketInteractorTag magAttachPoint;

    private bool shooting;
    private AudioSource shotAudio;

    public void ShootBullet()
    {
        smokeParticles.Play();
        GetComponent<AudioSource>().Play();

        GameObject bullet = null;
        if (bulletPool.Count < maxBulletsInScene)
        {
            bullet = Instantiate(bulletPrefab, shootingPoint.position + shootingPoint.forward * 0.5f, shootingPoint.rotation);
            bulletPool.Add(bullet);
        }
        else
        {
            bullet = bulletPool.Find(x => x.GetComponent<Bullet>().waiting);
            if (bullet != null)
            {
                bullet.GetComponent<Bullet>().ActivateBullet();
                bullet.transform.position = shootingPoint.position + shootingPoint.forward * 0.5f;
                bullet.transform.rotation = shootingPoint.rotation;
            }
            else
            {
                bullet = Instantiate(bulletPrefab, shootingPoint.position + shootingPoint.forward * 0.5f, shootingPoint.rotation);
                bullet.GetComponent<Bullet>().killYourself = true;
            }
        }

        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * shootingForce, ForceMode.VelocityChange);
        currentMag.BulletsLeft--;
        if (currentMag.BulletsLeft <= 0)
        {
            shooting = false;
            UpdateAnimator();
        }
    }

    public void EjectShell()
    {
        var shell = Instantiate(shellPrefab, shellEjectionPoint.position, Quaternion.LookRotation(-shellEjectionPoint.transform.right));
        shell.GetComponent<Rigidbody>().AddForce(shellEjectionPoint.transform.forward * shellEjectionForce, ForceMode.VelocityChange);
    }

    public void UpdateShootingState(bool state)
    {
        if (currentMag == null || currentMag.BulletsLeft <= 0) shooting = false;
        else shooting = state;

        UpdateAnimator();
    }

    public void UpdateMag()
    {
        var mag = magAttachPoint.currentAttachedObject;
        if (mag != null) currentMag = mag.GetComponent<Mag>();
        else currentMag = null;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Shooting", shooting);
    }

    void Start()
    {
        UpdateMag();
        shotAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }
}
