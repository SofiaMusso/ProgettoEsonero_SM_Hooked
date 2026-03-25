using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CardMovement : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
   
    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector2 originalLocalPointerPosition;
    private Vector3 originalPanelLocalPosition;
    private Vector3 originaleScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private int currentState = 0;

    [SerializeField] private float selectScale = 1.1f;
    [SerializeField] private Vector2 cardPlay;
    [SerializeField] private Vector3 playPosition;
    [SerializeField] private GameObject glowEffect;
    [SerializeField] private GameObject playArrow;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        //Get original data
        originaleScale = rectTransform.localScale;
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

                if (Mouse.current.leftButton.isPressed) // Check if mouse button is released
                {
                    TransitionToStateZero();
                }

                break;

        }
    }

    private void TransitionToStateZero() // Sets everything back to it's original state
    {
        currentState = 0;

        rectTransform.localPosition = originalPosition;  //Reset position
        rectTransform.localRotation = originalRotation; //Reset rotation
        rectTransform.localScale = originaleScale; //Reset scale

        glowEffect.SetActive(false);
        playArrow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        Debug.Log("PointerEnter"); 

        if (currentState == 0)
        {
            originalPosition = rectTransform.localPosition;
            originalRotation = rectTransform.localRotation;
            originaleScale = rectTransform.localScale; 

            currentState = 1;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("PointerExit");

        if (currentState == 1)
        {
            TransitionToStateZero();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("PointerDown");

        if (currentState == 1)
        {
            currentState = 2;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out originalLocalPointerPosition);
            originalPanelLocalPosition = rectTransform.localPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Drag");

        if (currentState == 2)
        {
            Vector2 localPointerPosition;

            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, eventData.pressEventCamera, out localPointerPosition))
            {
                rectTransform.position = Input.mousePosition;

                //localPointerPosition /= canvas.scaleFactor;

                //Vector3 offsetToOriginal = localPointerPosition - originalLocalPointerPosition;
                //rectTransform.localPosition = originalPanelLocalPosition + offsetToOriginal;

                if (rectTransform.localPosition.y > cardPlay.y)
                {
                    currentState = 3;
                    playArrow.SetActive(true);
                    rectTransform.localPosition = playPosition;
                }
            }
        }
    }

    private void HandleHoverState()
    {
        glowEffect.SetActive(true);
        rectTransform.localScale = originaleScale * selectScale;
    }

    private void HandleDragState()
    {
        //Set the card's rotation to zero

        rectTransform.localRotation = Quaternion.identity;
    }

    private void HandlePlayState()
    {
        rectTransform.localPosition = playPosition;

        //Set the card's rotation to zero
        rectTransform.localRotation = Quaternion.identity;

        if (Mouse.current.leftButton.ReadValue() > cardPlay.y)
        {
            currentState = 2;
            playArrow.SetActive(false);
        }
    }

}
