using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public Text hpText;
    public GameObject LoseGame;


    public int health;

    void Start()
    {
        UpdateHpText();
    }

    private void Update()
    {
        UpdateHpText();
    }
    public void ApllyDamage(int damage)
    {
        health -= damage;
        UpdateHpText();

        if (health <= 0f)
        {
            Time.timeScale = 0f;
            LoseGame.SetActive(true);
        }
    }
    public void UpdateHpText()
    {
        hpText.text = health.ToString() + "/ 100";
    }

}
