using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class Pressureplatereal : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    [SerializeField] GameObject text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        buttons.SetActive(false);
        text.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
       buttons.SetActive(true);
        text.SetActive(true);
    }
}
