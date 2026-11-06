using UnityEngine;
using System.IO.Ports;

public class ButtonCubeMover : MonoBehaviour
{
    [Header("Serial")]
    public string portName = "COM12";
    public int baudRate = 9600;

    [Header("Movement")]
    public float speed = 4f;      // units per second
    public float minX = -5f;
    public float maxX = 5f;

    SerialPort sp;
    string buffer = "";
    int left = 0;
    int right = 0;

    void Start()
    {
        try
        {
            sp = new SerialPort(portName, baudRate);
            sp.DtrEnable = true;
            sp.Open();
            Debug.Log("Opened " + portName);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Could not open " + portName + ": " + e.Message);
        }
    }

    void Update()
    {
        ReadSerial();

        float direction = right - left;   // -1, 0, or +1
        Vector3 pos = transform.position;
        pos.x += direction * speed * Time.deltaTime;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }

    void ReadSerial()
    {
        if (sp == null || !sp.IsOpen) return;

        try
        {
            if (sp.BytesToRead > 0)
                buffer += sp.ReadExisting();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("Serial read error: " + e.Message);
            return;
        }

        int lastNewline = buffer.LastIndexOf('\n');
        if (lastNewline < 0)
        {
            if (buffer.Length > 256) buffer = "";
            return;
        }

        string complete = buffer.Substring(0, lastNewline);
        buffer = buffer.Substring(lastNewline + 1);

        // Use the newest valid line, formatted "left,right"
        string[] lines = complete.Split('\n');
        for (int i = lines.Length - 1; i >= 0; i--)
        {
            string[] parts = lines[i].Trim().Split(',');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int l) &&
                int.TryParse(parts[1], out int r))
            {
                left = l;
                right = r;
                break;
            }
        }
    }

    void OnDestroy()
    {
        if (sp != null && sp.IsOpen) sp.Close();
    }
}