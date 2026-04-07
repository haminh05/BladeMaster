using UnityEngine;
using UnityEngine.UI;

public class ShopItemUI : MonoBehaviour
{
    public string productID;
    private Button btn;

    void Awake()
    {
        btn = GetComponent<Button>();
    }

    void OnEnable()
    {
        btn.onClick.AddListener(OnClick);
    }

    void OnDisable()
    {
        btn.onClick.RemoveListener(OnClick);
    }

    void OnClick()
    {
        IAPManager.Instance.BuyProduct(productID);
    }
}