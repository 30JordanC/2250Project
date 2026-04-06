using UnityEngine;
using UnityEngine.UI;

public class PipeTile : MonoBehaviour
{
    // Connections in order: Top, Right, Bottom, Left
    public bool[] connections = new bool[4];

    private int rotation = 0; // 0, 90, 180, 270
    private Image image;

    public void Init()
    {
        image = GetComponent<Image>();
    }

    public void Rotate()
    {
        rotation = (rotation + 90) % 360;
        transform.localEulerAngles = new Vector3(0, 0, -rotation);

        // Shift connections array to match rotation
        bool temp = connections[3];
        connections[3] = connections[2];
        connections[2] = connections[1];
        connections[1] = connections[0];
        connections[0] = temp;
    }

    // Returns whether this tile connects in a direction
    // 0=Top 1=Right 2=Bottom 3=Left
    public bool Connects(int direction) => connections[direction];
}
