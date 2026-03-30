using UnityEngine;

public class MovementTest : MonoBehaviour
{
    void Update()
    {
        Debug.Log("W pressed: " + Input.GetKey(KeyCode.W));
        
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward * 5f * Time.deltaTime;
        }
    }
}