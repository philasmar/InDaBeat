﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using static LyricResponse;
using UnityEngine.UI;
using System.Text;
using System.IO;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;

public class CubeActions : MonoBehaviour
{
    public GameObject music;
    AudioSource audio;
    WebUtils webUtils;
    List<LyricLine> songLyrics;
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
        if (songLyrics != null)
        {
            List<LyricLine> lyr = songLyrics.Where(x => x.time < Convert.ToDouble(audio.time) - 0.5).ToList();
            if(lyr.Count > 0)
            {
                lyrics.text = lyr.Last().text;
            }
        }
    }

    void Interact(RaycastHit hit)
    {
        if (hit.collider.name.Equals("Cube"))
        {
            //songLyrics = webUtils.getTopLyrics(Properties.songsList[0]);
            songLyrics = convertToText();
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

    List<LyricLine> convertToText()
    {
        StreamReader inp_stm = new StreamReader("Assets/Resources/" + Properties.lyricsFile[0] + ".lrc");
        List<LyricLine> inp_ln = new List<LyricLine>();
        while (!inp_stm.EndOfStream)
        {
            string line = inp_stm.ReadLine();
            if (isValidReg(line))
            {
                var culture = new CultureInfo("en-US");
                var formats = new string[] {
                        @"mm\:ss\.ff"
                    };
                string timeTxt = line.Substring(0, line.IndexOf("]") + 1).Replace("[", "").Replace("]", "");

                inp_ln.Add(new LyricLine(TimeSpan.ParseExact(timeTxt, formats, culture.NumberFormat).TotalSeconds, line.Substring(line.IndexOf("]") + 1)));
                // Do Something with the input. 
            }
        }

        inp_stm.Close(); inp_stm.Close();
        return inp_ln;
    }

    bool isValidReg(string str)
    {
        Regex rgx = new Regex(@"^\[[0-9]{2}:[0-9]{2}\.[0-9]{2}\].*$");
        return rgx.IsMatch(str);
    }
}
