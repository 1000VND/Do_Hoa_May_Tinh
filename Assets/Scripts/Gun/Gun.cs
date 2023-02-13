using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject fpsHand;
    public Text ammoText;
    private AudioSource _AudioSource;
    public AudioClip shootSound;
    private GameObject[] napDan;
    private GameObject[] danConLai;
    private int danDaSuDung;
    private int danDaBan = 0;

    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineMax, magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    //bools 
    bool shooting, readyToShoot, reloading;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;

    //Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;

    void Start()
    {
        _AudioSource = GetComponent<AudioSource>();
        napDan = GameObject.FindGameObjectsWithTag("NapDan");
    }

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();
        fpsHand.GetComponent<Animator>().SetBool("fire", shooting);
        fpsHand.GetComponent<Animator>().SetBool("reload", reloading);
        if (bulletsLeft == 0 && !reloading)
        {
            Reload();
        }
        ammoText.text = bulletsLeft + "/" + magazineMax;

        danConLai = GameObject.FindGameObjectsWithTag("NapDan");

        danDaSuDung = napDan.Length - danConLai.Length;

        magazineMax = 100 - danDaBan + 100 * danDaSuDung;
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && magazineSize <= magazineMax && !reloading) Reload();

        //Shoot
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    public void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, direction, out hit, range))
        {
            Debug.Log(hit.transform.name);

            HealthAI target = hit.transform.GetComponent<HealthAI>();
            if (target != null)
            {
                target.ApllyDamage(damage);
            }
        }

        //Graphics
        GameObject impactGO = Instantiate(bulletHoleGraphic, hit.point, Quaternion.LookRotation(hit.normal));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);

        Destroy(impactGO, 2f);
        PlayShootSound();
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload()
    {

        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        int left = magazineSize - bulletsLeft;
        danDaBan += left;
        bulletsLeft = magazineSize;
        magazineMax -= left;
        reloading = false;
    }

    private void PlayShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);
    }

}