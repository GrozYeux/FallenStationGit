using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : CharacterStats
{
    GameObject damagePanel;
    private void Start()
    {
        damagePanel = GameObject.Find("Canvas/DamagePanel");
        damagePanel.SetActive(false);

    }
    protected override void Die()
    {
        base.Die();
        Debug.Log(transform.name + " died.");
        SaveSystem.DeleteCodex();
        SaveSystem.DeletePlayer();

        //PanelMort.setActive(True);
        //reload the scene 
        SceneManager.LoadScene("Menu");
    }

    protected override void Hurt(float newAlpha)
    {

        damagePanel.SetActive(true);
        Image image = damagePanel.GetComponent<Image>();
        var tempColor = image.color;
        tempColor.a = newAlpha;
        image.color = tempColor;
    }
}