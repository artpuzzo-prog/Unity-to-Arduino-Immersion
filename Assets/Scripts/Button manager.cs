using UnityEngine;

public class Buttonmanager : MonoBehaviour
{
    [SerializeField] GameObject buttons;
    [SerializeField] AudioClip batsound;
    [SerializeField] GameObject text;

    public AudioSource audioSource;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void OnDisablehud(bool statement)
    {
       if (statement)
        {
            audioSource.PlayOneShot(batsound);
        }
      buttons.SetActive(false);
        text.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

