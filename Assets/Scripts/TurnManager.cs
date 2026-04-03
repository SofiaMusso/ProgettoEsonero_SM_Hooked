using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public TurnPhase currentPhase;

    private bool hasDrawn;

    public DeckManager deckManager;
    public HandManager handManager;
    public BoardManager boardManager;

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

    public enum TurnPhase
    {
        Draw,
        Main,      // play cards
        Combat,    // creatures attack
        End
    }

    private void Start()
    {
        StartTurn();
    }

    public void StartTurn()
    {
        currentPhase = TurnPhase.Draw;
        DrawPhase();
    }

    void DrawPhase()
    {
        if (!hasDrawn)
        {
            deckManager.DrawCard(handManager);
            hasDrawn = true;
            currentPhase = TurnPhase.Main;
        }
    }

    public void EndTurn()
    {
        if (hasDrawn)
        {
            currentPhase = TurnPhase.Combat;
            StartCoroutine(CombatPhase());
        }
    }

    IEnumerator CombatPhase()
    {
        yield return boardManager.ResolveCombat();

        currentPhase = TurnPhase.End;

        yield return new WaitForSeconds(1f);

        hasDrawn = false;

        StartTurn();
    }
}

