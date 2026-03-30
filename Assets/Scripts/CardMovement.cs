using CardManager;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
   
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private int currentState = 0;
    //public float moveHandDown;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlayY;
    /*[SerializeField] private Vector2 cardPlay1;
    [SerializeField] private Vector2 cardPlay2;
    [SerializeField] private Vector2 cardPlay3;
    [SerializeField] private Vector2 cardPlay4;
    [SerializeField] private Vector3 playPosition1;
    [SerializeField] private Vector3 playPosition2;
    [SerializeField] private Vector3 playPosition3;
    [SerializeField] private Vector3 playPosition4;*/
    [SerializeField] private GameObject glowEffect;

    public static CardMovement currentCardInPlay;
    public static bool isInPlay;
    public static bool isDragging = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();

        //Get original data
        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.localPosition; 
        originalRotation = rectTransform.localRotation;

    }
    void Update()
    {
        switch (currentState)
        {
            case 1:
                HandleHoverState();
                break;

            case 2:
                HandleDragState();

                if (!Mouse.current.leftButton.isPressed) // Check if mouse button is released
                {
                    TransitionToStateZero();
                }

                break;

            case 3:
                HandlePlayState();

                if (!Mouse.current.leftButton.isPressed) // Check if mouse button is released
                {
                    TransitionToStateZero();

                }

                break;

        }
    }

    private void TransitionToStateZero() // Sets everything back to it's original state
    {
        currentState = 0;
        currentCardInPlay = null;
        canvasGroup.blocksRaycasts = true;
        isDragging = false;

        rectTransform.localPosition = originalPosition;  //Reset position
        rectTransform.localRotation = originalRotation; //Reset rotation
        rectTransform.localScale = originalScale; //Reset scale

        glowEffect.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originalScale = rectTransform.localScale; 

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            TransitionToStateZero();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (currentState == 1)
        {
            currentState = 2;

            isDragging = true;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;

        if (currentState == 2)
        {
            Vector2 localPointerPosition;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.position = Input.mousePosition;

                if (rectTransform.localPosition.y > cardPlayY.y)
                {
                    currentState = 3;
                   // rectTransform.localPosition = playPosition;
                }
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originalScale * selectScale;
    }

    private void HandleDragState()
    {
        //Set the card's rotation to zero
        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        currentCardInPlay = this;

        //Set the card's rotation to zero
        rectTransform.localRotation = Quaternion.identity;

        if (Mouse.current.leftButton.ReadValue() < cardPlayY.y)
        {
            currentState = 2;
        }
    }
}
