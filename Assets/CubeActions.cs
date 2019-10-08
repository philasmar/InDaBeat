using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using static LyricResponse;
using UnityEngine.UI;
using System.Text;

public class CubeActions : MonoBehaviour
{
    public GameObject music;
    AudioSource audio;
    WebUtils webUtils;
    List<string> songLyrics;
    public Text lyrics;

    // Start is called before the first frame update
    void Start()
    {
        audio = music.GetComponent<AudioSource>();
        webUtils = new WebUtils();
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
        if(songLyrics != null)
        {
            float ratio = audio.time / audio.clip.length;
            int index = (int)(ratio * songLyrics.Count);
            lyrics.text = songLyrics[index];
        }
    }

    void Interact(RaycastHit hit)
    {
        if (hit.collider.name.Equals("Cube"))
        {

            songLyrics = webUtils.getTopLyrics(Properties.songsList[0]);
            if (audio.isPlaying)
            {
                audio.Pause();
            }
            else
            {
                audio.Play();
                convertToText();
            }
        }
    }

    string convertToText()
    {
        TextAsset txt = (TextAsset)Resources.Load("Killshot", typeof(TextAsset));
        Debug.Log(txt.text);
        return txt.text;
    }
}
