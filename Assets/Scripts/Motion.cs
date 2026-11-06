using UnityEngine;
using System.IO.Ports;

public class Motion : MonoBehaviour
{
    SerialPort sp = new SerialPort("COM12", 9600);

    void Start ()
    {
        sp.Open();
        sp.ReadTimeout = 1;
    }
    private void Update()  {

        if (sp.IsOpen)
        {
            try
            {
                if (sp.ReadByte() == 1)
                {
                    transform.Translate(Vector3.left * Time.deltaTime * 5);
                }
                if (sp.ReadByte() == 2)
                {
                    transform.Translate(Vector3.right * Time.deltaTime * 5);
                }
            } catch (System.Exception) { 
            }
        }

    }

}
