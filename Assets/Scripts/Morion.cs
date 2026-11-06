using UnityEngine;
using System.IO.Ports;
using System.Runtime.CompilerServices;

public class PotCubeMover : MonoBehaviour
{
    [Header("Serial")]
    public string portName = "COM3";
    public int baudRate = 9600;

    [Header("Movement range (world units)")]
    public float minX = -5f;
    public float maxX = 5f;

    public float minY = -5f;
    public float maxY = 5f;


    [Header("Smoothing")]
    public float smoothSpeed = 10f;

    [Header("Pot calibration")]
    public int potMin = 0;
    public int potMax = 1023;   // analogRead range on the Lotus

    SerialPort sp;
    string buffer = "";
    float targetX;

    float targetY;
    void Start()
    {
        targetX = transform.position.x;

        targetY = transform.position.y;

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
        ReadPot();
        

        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, targetX, smoothSpeed * Time.deltaTime);
   
        pos.y = Mathf.Lerp(pos.y, targetY, smoothSpeed * Time.deltaTime);
        transform.position = pos;
    }

    void ReadPot()
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

        // Use only complete lines; keep any partial line for next frame
        int lastNewline = buffer.LastIndexOf('\n');
        if (lastNewline < 0)
        {
            if (buffer.Length > 256) buffer = "";   // safety: discard junk
            return;
        }

        string complete = buffer.Substring(0, lastNewline);
        buffer = buffer.Substring(lastNewline + 1);

        // Take the newest line so the cube never lags behind
        string[] lines = complete.Split('\n');
        for (int i = lines.Length - 1; i >= 0; i--)
        {
            if (int.TryParse(lines[i].Trim(), out int raw))
            {
                float t = Mathf.InverseLerp(potMin, potMax, raw);
                targetX = Mathf.Lerp(minX, maxX, t);
                Debug.Log("Pot: " + raw);   // remove once it works
                break;
            }
        }
    }

    void OnDestroy()
    {
        if (sp != null && sp.IsOpen) sp.Close();
    }
}