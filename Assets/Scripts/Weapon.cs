using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Animator anim;
    private AudioSource _AudioSource;

    public float range = 100f;
    public int bulletsPerMag = 30; //1 băng đạn
    public int bulletsLeft = 300;// tổng số đạn có

    public int currentBullets;//những viên đạn hiện tại trong băng đạn

    public enum ShootMode { Auto, Semi }
    public ShootMode shootingMode;

    public Transform shootPoint;
    public GameObject hitParticles;

    public ParticleSystem muzzleFlash;
    public AudioClip shootSound;



    public float fireRate = 0.1f;
    public int damage = 20;

    float fireTimer;

    private bool isReloading;
    private bool shootInput;

    private Vector3 originalPosition;
    public Vector3 aimPosition;
    public float aodSpeed = 8f;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();

        currentBullets = bulletsPerMag;

        originalPosition = transform.localPosition;

    }

    // Update is called once per frame
    void Update()
    {
        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

    }

    void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        isReloading = info.IsName("Reload");


        //if (info.IsName("Fire")) anim.SetBool("Fire", false);
    }


    public void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading)
            return;

        RaycastHit hit;

        if (Physics.Raycast(shootPoint.position, shootPoint.transform.forward, out hit, range))
        {

            GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            Destroy(hitParticleEffect, 1f);

            if (hit.transform.GetComponent<GameUI>())
            {
                hit.transform.GetComponent<GameUI>().TakeDamage(damage);
            }
        }
        anim.CrossFadeInFixedTime("demo_combat_shoot", 0.1f);
        muzzleFlash.Play();
        PlayShootSound();
        currentBullets--;
        fireTimer = 0.0f;
    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag - currentBullets;
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;
    }

    public void DoReLoad()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (isReloading) return;

        anim.CrossFadeInFixedTime("Reload", 0.01f);
    }
    private void PlayShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);
        // _AudioSource.clip = shootSound;
        // _AudioSource.Play(); 
    }
}
