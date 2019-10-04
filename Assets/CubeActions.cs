using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeActions : MonoBehaviour
{
    public GameObject music;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = music.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                Interact(hit);
        }
    }

    void Interact(RaycastHit hit)
    {
        if (hit.collider.name.Equals("Cube"))
        {
            if (audio.isPlaying)
            {
                audio.Pause();
            }
            else
            {
                audio.Play();
            }
        }
    }
}
