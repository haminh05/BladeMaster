using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class KnifeController : MonoBehaviour
{
    public float throwForce = 20f;

    private Rigidbody2D rb;
    private Collider2D col;

    //private bool isFlying = false;
    private bool isActive = true;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        col.enabled = false;
    }

    public void Throw()
    {
        if (!isActive) return;

        col.enabled = true;

        rb.AddForce(Vector2.up * throwForce, ForceMode2D.Impulse);

        //isFlying = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isActive) return;

        isActive = false;

        if (collision.gameObject.CompareTag("Target"))
        {
            rb.linearVelocity = Vector2.zero;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.angularVelocity = 0f;
            
            transform.SetParent(collision.transform);

            StageManager.Instance.OnKnifeHit();
            StageUIManager.Instance.AddScore(1);
            SoundManager.Instance.PlayHitTarget();
            Debug.Log("Hit Target!");

            // spawn dao mới
            KnifeSpawner.Instance.SpawnKnife();
        }

        else if (collision.gameObject.CompareTag("Knife"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -2);
            gameObject.tag = "Untagged";
            col.enabled = false;
            SoundManager.Instance.PlayHitKnife();
            SettingsManager.Instance.Vibrate();
            HealthSystem.Instance.PlayerFailed();
            Destroy(gameObject);
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActive) return;
        if(collision.gameObject.CompareTag("Apple"))
        {
            StageUIManager.Instance.AddApple(1);
            Debug.Log("Hit Target Trigger!");
            Destroy(collision.gameObject);
        }
    }
}