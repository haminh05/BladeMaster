using UnityEngine;

public class KnifeSpawner : MonoBehaviour
{
    public static KnifeSpawner Instance;

    public GameObject knifePrefab;
    public Transform spawnPoint;

    private KnifeController currentKnife;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnKnife();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowKnife();
        }
    }

    public void SpawnKnife()
    {
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