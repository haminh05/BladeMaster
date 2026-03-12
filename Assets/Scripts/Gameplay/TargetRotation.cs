using UnityEngine;

public class TargetRotation : MonoBehaviour
{
    private float rotationSpeed;

    public void Init(float startSpeed)
    {
        rotationSpeed = startSpeed;
    }

    public void SetRotation(float speed)
    {
        rotationSpeed = speed;
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
}