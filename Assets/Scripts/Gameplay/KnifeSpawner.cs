using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    public static KnifeSpawner Instance;
    public Transform spawnPoint;

    // knifePrefab không cần gán trong Inspector nữa
    private GameObject knifePrefab;
    private KnifeController currentKnife;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        LoadEquippedKnife();
        SpawnKnife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ThrowKnife();
    }

    void LoadEquippedKnife()
    {
        KnifeData data = InventoryManager.Instance.GetEquippedKnifeData();
        if (data != null && data.knifePrefab != null)
            knifePrefab = data.knifePrefab;
        else
            Debug.LogWarning("KnifeSpawner: Không tìm thấy dao equip, check InventoryManager!");
    }

    public void SpawnKnife()
    {
        if (knifePrefab == null) return;
        GameObject knife = Instantiate(knifePrefab, spawnPoint.position, Quaternion.identity);
        currentKnife = knife.GetComponent<KnifeController>();
    }

    void ThrowKnife()
    {
        if (currentKnife != null)
        {
            currentKnife.Throw();
            currentKnife = null;
        }
    }
}