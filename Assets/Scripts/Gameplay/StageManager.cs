using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public StageConfig[] stages;
    public Transform targetSpawnPoint;
    private int currentStage = 0;
    public int knivesHit = 0;
    private GameObject currentTarget;
    private Coroutine rotationCoroutine;

    public static StageManager Instance;

    void Start()
    {
        StartStage();
    }
    void Awake()
    {
        Instance = this;
    }
    public void StartStage()
    {
        knivesHit = 0;
        int displayStage = currentStage + 1; // Hiển thị stage bắt đầu từ 1
        StageUIManager.Instance.SetStage(displayStage);
        
        StageConfig stage = stages[currentStage];
        StageUIManager.Instance.SetInitialDisplayedKnifeCount(stage.knivesToHit);

        if (currentStage == 0 || stages[currentStage - 1].isBoss)
        {
            StageUIManager.Instance.SetupBossProgress(stages, currentStage);
        }
        if (stage.isBoss)
        {
            StageUIManager.Instance.ClearBossProgress();
            StageUIManager.Instance.ShowBossBanner(stage.bossName, stage.bossIconPrefab);
        }
        else
        {
            StageUIManager.Instance.HideBossBanner();
        }
        Debug.Log($"Starting Stage {currentStage + 1}: Need to hit {stage.knivesToHit} knives.");
        // spawn target
        currentTarget = Instantiate(stage.targetPrefab, targetSpawnPoint.position, Quaternion.identity);

        TargetRotation rot = currentTarget.GetComponent<TargetRotation>();
        SpawnStartingKnives(stage);
        SpawnApples(stage);
        rot.Init(stage.startRotationSpeed);

        if (stage.randomRotation)
        {
            rotationCoroutine = StartCoroutine(RandomRotation(stage, rot));
        }
    }

    IEnumerator RandomRotation(StageConfig stage, TargetRotation rot)
    {
        while (true)
        {
            yield return new WaitForSeconds(stage.rotationChangeInterval);

            float speed = Random.Range(stage.minRotationSpeed, stage.maxRotationSpeed);

            if (stage.allowReverse && Random.value > 0.5f)
                speed *= -1;

            rot.SetRotation(speed);
        }
    }
    public void OnKnifeHit()
    {
        knivesHit++;
        StageUIManager.Instance.DecrementDisplayedKnifeCount();
        StageConfig stage = stages[currentStage];

        if (knivesHit >= stage.knivesToHit)
        {
            Debug.Log("Stage Clear!");
            NextStage();
        }
    }
    public void NextStage()
    {
        StageConfig clearedStage = stages[currentStage];
        bool wasBoss = clearedStage.isBoss;

        // Unlock dao thưởng nếu là boss stage
        if (wasBoss && clearedStage.rewardKnife != null)
        {
            InventoryManager.Instance.UnlockKnife(clearedStage.rewardKnife.id);
            Debug.Log($"Unlocked reward knife: {clearedStage.rewardKnife.name}");
        }

        if (rotationCoroutine != null)
            StopCoroutine(rotationCoroutine);

        Destroy(currentTarget);

        currentStage++;
        SaveSystem.SaveMaxStage(currentStage + 1);

        if (!wasBoss)
            StageUIManager.Instance.AdvanceBossProgress();

        if (currentStage >= stages.Length)
        {
            Debug.Log("Game Completed!");
            return;
        }

        StartStage();
    }

    void SpawnStartingKnives(StageConfig stage)
    {
        if (stage.startingKnives <= 0) return;

        CircleCollider2D targetCol = currentTarget.GetComponent<CircleCollider2D>();
        float worldRadius = targetCol.radius * currentTarget.transform.localScale.x;

        // Lấy knifeSize từ prefab (chưa instantiate) — dùng SpriteRenderer thay bounds
        float knifeHalfHeight = stage.knifePrefab.transform.localScale.y
                                * stage.knifePrefab.GetComponent<SpriteRenderer>().sprite.bounds.extents.y;

        // Radius trong LOCAL space của target
        float localRadius = (worldRadius + knifeHalfHeight + 0.05f)
                            / currentTarget.transform.localScale.x;

        for (int i = 0; i < stage.startingKnives; i++)
        {
            float angle = (360f / stage.startingKnives) * i;

            GameObject knife = Instantiate(stage.knifePrefab);
            knife.transform.localScale = stage.knifePrefab.transform.localScale;
            knife.transform.SetParent(currentTarget.transform);

            // Giữ đúng world scale sau khi parent


            // Vị trí local xung quanh tâm target
            float spawnYOffset = -3.244561f;

            Vector3 localPos =
     Quaternion.Euler(0, 0, angle) * Vector3.up * (localRadius + spawnYOffset);

            knife.transform.localPosition = localPos;

            // Quay mũi dao ra ngoài (ngược lại nếu sprite ngược)
            knife.transform.up =
            (currentTarget.transform.position - knife.transform.position);

            // Tắt physics
            Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
                rb.simulated = true;
            }

            // Tắt collider để không trigger ngay
            Collider2D col = knife.GetComponent<Collider2D>();
            if (col != null) col.enabled = true;
        }
    }

    void SpawnApples(StageConfig stage)
    {
        if (stage.appleCount <= 0) return;

        CircleCollider2D targetCol = currentTarget.GetComponent<CircleCollider2D>();
        float worldRadius = targetCol.radius * currentTarget.transform.localScale.x;

        float appleHalfSize = stage.applePrefab.transform.localScale.x
                              * stage.applePrefab.GetComponent<SpriteRenderer>().sprite.bounds.extents.x;

        // Radius trong LOCAL space của target
        float localRadius = (worldRadius + appleHalfSize + 0.05f)
                            / currentTarget.transform.localScale.x;

        List<float> usedAngles = new List<float>();

        for (int i = 0; i < stage.appleCount; i++)
        {
            float angle;
            int maxTry = 100;
            Debug.Log("Spawn apple " + i);
            // Tránh apple chồng lên nhau
            do
            {
                angle = Random.Range(0f, 360f);
                maxTry--;
            } while (usedAngles.Exists(a => Mathf.Abs(Mathf.DeltaAngle(a, angle)) < 30f) && maxTry > 0);

            usedAngles.Add(angle);

            GameObject apple = Instantiate(stage.applePrefab);
            apple.transform.localScale = stage.applePrefab.transform.localScale;
            apple.transform.SetParent(currentTarget.transform);
            

            Vector3 localPos = Quaternion.Euler(0, 0, angle) * Vector3.up * localRadius;
            apple.transform.localPosition = localPos;

            // Apple quay mặt ra ngoài
            apple.transform.up = apple.transform.position - currentTarget.transform.position;
        }
    }
}