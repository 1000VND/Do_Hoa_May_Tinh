using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WeaponAI : MonoBehaviour
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
    public TextMeshProUGUI text;

    public float fireRate = 0.1f;
    public int damage = 20;

    float fireTimer;

    private bool isReloading;
    private bool isAiming;
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
        switch (shootingMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
                break;

            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
                break;
        }

        if (shootInput)
        {
            if (currentBullets > 0)
                Fire();
            else if (bulletsLeft > 0)
                DoReLoad();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentBullets < bulletsPerMag && bulletsLeft > 0)
                DoReLoad();
        }


        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        AimDownSights();

        text.SetText(currentBullets + " / " + bulletsLeft);
    }

    void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

         isReloading = info.IsName("Reload");
        anim.SetBool("Aim", isAiming);

        //if (info.IsName("Fire")) anim.SetBool("Fire", false);
    }

    private void AimDownSights()
    {
        if (Input.GetButton("Fire2") && !isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * aodSpeed);
            isAiming = true;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aodSpeed);
            isAiming = false;
        }
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

            if (hit.transform.GetComponent<HealthAI>())
            {
                hit.transform.GetComponent<HealthAI>().ApllyDamage(damage);

            }
        }
        anim.CrossFadeInFixedTime("Fire", 0.1f);
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
