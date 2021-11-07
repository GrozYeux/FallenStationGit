using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    GameObject damagePanel ;
    private float regenDelta = 0.0f;
    public float regenFrequency = 4.0f;

    private void Start()
    {
        damagePanel = GameObject.Find("Canvas/DamagePanel");
        modifyDamagePanel(0.0f);
    }
    private void Update()
    {
        if (currentHealth < maxHealth)
        {
            Regen();
        }
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log(transform.name + " died.");
        
        SaveSystem.DeleteCodex();
        SaveSystem.DeletePlayer();
        Destroy(this.gameObject);
        //PanelMort.setActive(True);
        //reload the scene 
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
        SceneManager.LoadScene("Menu");
    }

    protected override void Hurt(float newAlpha )
    {
        modifyDamagePanel(newAlpha);
    }

    private void Regen()
    {
        //regenerate the player, accounting the frequency
        if (regenDelta > regenFrequency)
        {
            currentHealth += 10;

            modifyDamagePanel((maxHealth - currentHealth) / maxHealth);

            regenDelta = 0.0f;
        }
        else
        {
            regenDelta += Time.deltaTime;
        } 

    }
    protected override void Awake()
    {
        base.Awake();
        modifyDamagePanel(0.0f);
    }

    protected void modifyDamagePanel(float newAlpha)
    {
        //damagePanel = GameObject.Find("Canvas/DamagePanel");
        //damagePanel.SetActive(true);
        damagePanel = GameObject.Find("Canvas/DamagePanel");
        Image image = damagePanel.GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = newAlpha;
        image.color = tempColor;
    }
}
