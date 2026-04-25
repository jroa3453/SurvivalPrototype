using UnityEngine;
using TMPro;

public class Compass : MonoBehaviour
{
    public Transform player;
    public TMP_Text compassText;

    private void Update()
    {
       float angle = player.eulerAngles.y;

        if (angle > 315 || angle < 45)
            compassText.text = "N";
        else if (angle >= 45 && angle < 90)
            compassText.text = "NE";
        else if (angle >= 90 && angle < 135)
            compassText.text = "E";
        else if (angle >= 135 && angle < 180)
            compassText.text = "SE";
        else if (angle >= 180 && angle < 225)
            compassText.text = "S";
        else if (angle >= 225 && angle < 270)
            compassText.text = "SW";
        else if (angle >= 270 && angle < 315)
            compassText.text = "W";
        else
            compassText.text = "NW";

    }
}