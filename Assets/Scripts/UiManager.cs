using TMPro;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UiManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject panelWin;
    public GameObject panelLose;

    public GameObject panelOptions;
    private bool settingsOpen = false;

    public GameObject panelChangeTurnPlayer;
    public GameObject panelChangeTurnEnemy;

    public TextMeshProUGUI textDroplets;
    public TextMeshProUGUI textDmg;

    public TextMeshProUGUI textEnemyDroplets;
    public TextMeshProUGUI textEnemyDmg;

    private void Start()
    {
        mainMenu.SetActive(false);

        //Time.timeScale = 0;

        panelWin.SetActive(false);
        panelLose.SetActive(false);
        panelOptions.SetActive(false);

        panelChangeTurnPlayer.SetActive(false);
        panelChangeTurnEnemy.SetActive(false);
    }

    private void Update()
    {

        textDroplets.SetText(PlayerData.playerDroplets.ToString());
        textDmg.SetText(PlayerData.playerDmgPoints.ToString());

        textEnemyDroplets.SetText(PlayerData.enemyDroplets.ToString());
        textEnemyDmg.SetText(PlayerData.enemyDmgPoints.ToString());

    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void PlayerHasWon()
    {
        panelWin.SetActive(true);
    }

    public void PlayerHasLost()
    {
        panelLose.SetActive(true);
    }

    public void OpenSettings()
    {
        if (!settingsOpen) 
        { 
            panelOptions.SetActive(true);
            Time.timeScale = 0;

            settingsOpen = true;
        }
    }

    public void CloseSettings()
    {
        if (settingsOpen)
        {
            panelOptions.SetActive(false);
            Time.timeScale = 1;

            settingsOpen = false;
        }
    }

    public IEnumerator ChangeTurnP()
    {
        panelChangeTurnPlayer.SetActive(true);
        yield return new WaitForSeconds(1f);

        panelChangeTurnPlayer.SetActive(false);
    }

    public IEnumerator ChangeTurnE()
    {
        panelChangeTurnEnemy.SetActive(true);
        yield return new WaitForSeconds(1f);

        panelChangeTurnEnemy.SetActive(false);
    }
}
