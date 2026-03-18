using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;
    public Transform content;
    public TextMeshProUGUI pageLabel;
    public TextMeshProUGUI countLabel; // thêm field này, kéo txt vào đây
    public KnifeDatabase knifeDatabase;
    public float snapSpeed = 10f;
    public float dragThreshold = 80f;

    private float[] pages;
    private int currentPage = 0;
    private bool isSnapping = false;
    private float dragStartPos;

    private string[] pageLabels =
    {
        "GET FOR APPLES",
        "WATCH VIDEOS",
        "BOSS KNIVES",
        "CHALLENGE KNIVES"
    };

    // Thứ tự phải khớp với pageLabels
    private KnifeUnlockType[] pageTypes =
    {
        KnifeUnlockType.Apple,
        KnifeUnlockType.Ads,
        KnifeUnlockType.Boss,
        KnifeUnlockType.BossChallenge
    };

    void Start()
    {
        RectTransform viewportRT = scrollRect.viewport;
        float pageWidth = viewportRT.rect.width;
        float pageHeight = viewportRT.rect.height;

        int pageCount = content.childCount;

        for (int i = 0; i < pageCount; i++)
        {
            RectTransform pageRT = content.GetChild(i).GetComponent<RectTransform>();
            pageRT.sizeDelta = new Vector2(pageWidth, pageHeight);
        }

        RectTransform contentRT = content.GetComponent<RectTransform>();
        contentRT.sizeDelta = new Vector2(pageWidth * pageCount, pageHeight);

        pages = new float[pageCount];
        for (int i = 0; i < pageCount; i++)
            pages[i] = pageCount == 1 ? 0f : (float)i / (pageCount - 1);

        scrollRect.inertia = false;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;

        UpdateLabel();
    }

    void Update()
    {
        if (!isSnapping) return;

        float target = pages[currentPage];
        float current = scrollRect.horizontalNormalizedPosition;
        float next = Mathf.Lerp(current, target, Time.deltaTime * snapSpeed);
        scrollRect.horizontalNormalizedPosition = next;

        if (Mathf.Abs(next - target) < 0.001f)
        {
            scrollRect.horizontalNormalizedPosition = target;
            isSnapping = false;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragStartPos = eventData.position.x;
        isSnapping = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float delta = eventData.position.x - dragStartPos;

        if (delta < -dragThreshold && currentPage < pages.Length - 1)
            currentPage++;
        else if (delta > dragThreshold && currentPage > 0)
            currentPage--;

        isSnapping = true;
        UpdateLabel();
    }

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            isSnapping = true;
            UpdateLabel();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            isSnapping = true;
            UpdateLabel();
        }
    }

    void UpdateLabel()
    {
        if (pageLabel != null && currentPage < pageLabels.Length)
            pageLabel.text = pageLabels[currentPage];

        UpdateCountLabel();

        if (KnifeCollectionUI.Instance != null && currentPage < pageTypes.Length)
            KnifeCollectionUI.Instance.OnPageChanged(pageTypes[currentPage]);
    }

    void UpdateCountLabel()
    {
        if (countLabel == null || knifeDatabase == null) return;

        int total = knifeDatabase.knives.Length;
        int unlocked = knifeDatabase.knives
            .Count(k => InventoryManager.Instance.IsKnifeUnlocked(k.id));

        countLabel.text = $"{unlocked}/{total}";
    }
}