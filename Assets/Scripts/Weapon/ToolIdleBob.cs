using UnityEngine;

public class ToolIdleBob : MonoBehaviour
{
    public float bobSpeed = 1.5f;    // how fast it bobs
    public float bobAmount = 0.002f; // how much it moves

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.localPosition;
    }

    void Update()
    {
        float bob = Mathf.Sin(Time.time * bobSpeed) * bobAmount;
        transform.localPosition = startPosition + new Vector3(0, bob, 0);
    }
}