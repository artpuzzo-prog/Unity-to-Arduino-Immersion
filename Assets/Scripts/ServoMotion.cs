using UnityEngine;
using System.IO.Ports;

public class ServoController : MonoBehaviour
{
    public string portName = "COM12";
    public int baudRate = 9600;

    SerialPort sp;

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

    // Any button can call this and pass an angle
    public void SetAngle(int angle)
    {
        if (sp == null || !sp.IsOpen) return;
        angle = Mathf.Clamp(angle, 0, 180);
        sp.WriteLine(angle.ToString());
        Debug.Log("Sent angle " + angle);
    }

    void OnDestroy()
    {
        if (sp != null && sp.IsOpen) sp.Close();
    }
}