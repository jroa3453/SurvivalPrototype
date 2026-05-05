using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Light flashlight;
    public bool isOn = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isOn = !isOn;
            flashlight.enabled = isOn;
        }
    }
}