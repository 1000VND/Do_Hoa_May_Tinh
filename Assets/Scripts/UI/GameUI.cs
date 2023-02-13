using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    private PlayerInventory inventory;
    public TextMeshProUGUI countItems;
    public TextMeshProUGUI promptText;
    public GameObject prompt;

    private float health;
    private float lerpTimer;
    public float maxHealth = 100f;
    public float chipSpeed = 2f;
    public Image frontHealthBar;
    public Image backHealthBar;

    private float shield;
    public float maxShield = 70f;
    public Image frontShieldBar;
    public Image backShieldBar;

    public GameObject UI, gameUI, gameOver, winGame;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        shield = maxShield;
        inventory = GetComponent<PlayerInventory>();
        UI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        countItems.text = inventory.MedicalItems.ToString();

        health = Mathf.Clamp(health, 0, maxHealth);
        shield = Mathf.Clamp(shield, 0, maxShield);

        UpdateGameUI();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(Random.Range(5, 10));
        }

        if (Input.GetKeyDown(KeyCode.U) && inventory.MedicalItems > 0 && health < maxHealth)
        {
            Recover(20f);
            inventory.MedicalItemUsed += 1;
        }

        GameOver();

        GameWin();
       
    }

    public void UpdateGameUI()
    {
        float fillFH = frontHealthBar.fillAmount;
        float fillBH = backHealthBar.fillAmount;
        float hFractionH = health / maxHealth;
        if(fillBH > hFractionH)
        {
            frontHealthBar.fillAmount = hFractionH;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount =  Mathf.Lerp(fillBH, hFractionH, percentComplete);
        }

        if(fillFH < hFractionH)
        {
            backHealthBar.fillAmount = hFractionH;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            frontHealthBar.fillAmount = Mathf.Lerp(fillFH, backHealthBar.fillAmount, percentComplete);
        }

        float fillFS = frontShieldBar.fillAmount;
        float fillBS = backShieldBar.fillAmount;
        float hFractionS = shield / maxShield;
        if (fillBS > hFractionS)
        {
            frontShieldBar.fillAmount = hFractionS;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backShieldBar.fillAmount = Mathf.Lerp(fillBS, hFractionS, percentComplete);
        }

        //if (fillFS < hFractionS)
        //{
        //    frontShieldBar.fillAmount = hFractionS;
        //    lerpTimer += Time.deltaTime;
        //    float percentComplete = lerpTimer / chipSpeed;
        //    percentComplete = percentComplete * percentComplete;
        //    backShieldBar.fillAmount = Mathf.Lerp(fillFS, backShieldBar.fillAmount, percentComplete);
        //}
    }

    public void TakeDamage(float damage)
    {
        shield -= damage;
        if(shield <= 0)
        {
            health -= damage;
        }
        lerpTimer = 0f;
    }

    public void Recover(float recover)
    {
        health += recover;
        lerpTimer = 0f;
    }

    //public void RestoreShield(float shieldAmount)
    //{
    //    shield += shieldAmount;
    //    lerpTimer = 0f;
    //}

    public void UpdateText(string promptMessage)
    {
        promptText.text = promptMessage;
    }

    private void GameOver()
    {
        if(health <= 0)
        {
            gameUI.SetActive(false);
            gameOver.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    private void GameWin()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length <= 0)
        {
            gameUI.SetActive(false);
            winGame.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
