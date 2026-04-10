using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public TurnOwner currentTurn;
    public TurnPhase currentPhase;

    public bool hasDrawn;
    public bool playerCardIsAttacker;

    public DeckManager deckManager;
    public HandManager handManager;
    public BoardManager boardManager;
    public PlayerData playerData;
    public UiManager uiManager;

    public EnemyAI enemyAI;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public enum TurnOwner
    {
        Player,
        Enemy
    }

    public enum TurnPhase
    {
        Draw,
        Main,      // play cards
        Combat,    // creatures attack
        End
    }

    private void Start()
    {
        if (currentTurn == TurnOwner.Player)
        {
            currentTurn = TurnOwner.Enemy;
        }
        else
        {
            currentTurn = TurnOwner.Player;
        }
    }

    public void StartTurn()
    {
        if (currentTurn == TurnOwner.Player)
        {
            currentTurn = TurnOwner.Enemy;
            StartCoroutine(uiManager.ChangeTurnE());
        }
        else
        {
            currentTurn = TurnOwner.Player;
            StartCoroutine(uiManager.ChangeTurnP());
        }

        Debug.Log(currentTurn);

        if (currentTurn == TurnOwner.Player)
        {
            PlayerData.playerDroplets = PlayerData.playerDroplets + 1;
            Debug.Log("Player Droplets = " + PlayerData.playerDroplets);
            currentPhase = TurnPhase.Draw;
            DrawPhase();
        }
        else if (currentTurn == TurnOwner.Enemy)
        {
            PlayerData.enemyDroplets = PlayerData.enemyDroplets + 1;
            Debug.Log("Enemy Droplets = " + PlayerData.enemyDroplets);
            enemyAI.PlayTurn();
        }
    }

    void DrawPhase()
    {
        if (hasDrawn)
        {
            Debug.Log("Has Drawn Card");
            currentPhase = TurnPhase.Main;
            Debug.Log(currentPhase);
        }
    }

    public void EndTurn()
    {
        if (!hasDrawn)
        {
            Debug.Log("Hasn't drawn");
        }
        if (hasDrawn)
        {
            Debug.Log("Ended Turn");
            currentPhase = TurnPhase.Combat;
            StartCoroutine(CombatPhase());
        }
    }

    IEnumerator CombatPhase()
    {
        if (currentTurn == TurnOwner.Player)
        {
            playerCardIsAttacker = true;

            yield return boardManager.ResolveCombat(
                boardManager.playerSlots,
                boardManager.enemySlots
            );
        }
        else
        {
            playerCardIsAttacker = false; 

            yield return boardManager.ResolveCombat(
                boardManager.enemySlots,
                boardManager.playerSlots
            );
        }

        currentPhase = TurnPhase.End;

        yield return new WaitForSeconds(1f);

        hasDrawn = false;

        CheckForWin();

        StartTurn();
    }

    public void CheckForWin()
    {
        int playerDmg = PlayerData.playerDmgPoints;
        int enemyDmg = PlayerData.enemyDmgPoints;

        if (playerDmg >= 30)
        {
            Debug.Log("Player instant win");
            return;
        }

        if (enemyDmg >= 30)
        {
            Debug.Log("Enemy instant win");
            return;
        }

        int diff = Mathf.Abs(playerDmg - enemyDmg);

        Debug.Log("Damage difference = " + diff);

        if (diff >= 7)
        {
            if (playerDmg > enemyDmg)
            {
                Debug.Log("Player has won the round");
                uiManager.PlayerHasWon();
            }
            else
            {
                Debug.Log("Enemy has won the round");
                uiManager.PlayerHasLost();
            }
        }
    }
}

