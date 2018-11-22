using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUICard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    #region Variables
    [SerializeField] private RectTransform Rt_DragIcon;
    [SerializeField] private Image image;

    private Canvas canvas;
    private Vector3 originalPos;
    private Coroutine cGoToPosition;

    public ItemUICardHolder[] cardHolders;
    public bool isDragable = true;
    public Item item { get; private set; }
    #endregion

    #region Methods
    #region Unity
    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        originalPos = Rt_DragIcon.localPosition;
    }
    #endregion
    #region Drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isDragable)
            return;
        if (cGoToPosition != null)
            StopCoroutine(cGoToPosition);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragable)
            return;
        Rt_DragIcon.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragable)
            return;
        RectTransform dropZone;
        for (int i = 0; i < cardHolders.Length; i++) {
            dropZone = cardHolders[i].rect;
            if (CustomUtils.IsPosInRect(dropZone, eventData.position))
            {
                cardHolders[i].SetCard(this, originalPos);
                GoToPosition(dropZone.localPosition);
                return;
            }
        }
        GoToPosition(originalPos);
    }
    #endregion
    public void GoToPosition(Vector3 position)
    {   
        cGoToPosition = StartCoroutine(EGoToPosition(position, 0.5f));
    }

    public IEnumerator EGoToPosition(Vector3 newPosition,float time)
    {
        float elapsedTime = 0;
        Vector3 startPos = Rt_DragIcon.localPosition;
        float dist = Vector3.Distance(startPos, newPosition);
        dist = CustomUtils.Remap(dist, 0, 3000, 0, 1);
        time = time * dist;

        while (elapsedTime < time)
        {
            Rt_DragIcon.localPosition = Vector3.Lerp(startPos, newPosition, elapsedTime/time);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        Rt_DragIcon.localPosition = newPosition;
        originalPos = newPosition;
    }

    public void SetUp(Item item,ItemUICardHolder[] holders)
    {
        this.item = item;
        image.sprite = item.sprite;
        this.cardHolders = holders;
    }
    #endregion
}
