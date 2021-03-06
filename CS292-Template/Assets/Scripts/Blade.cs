﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blade : MonoBehaviour
{
    bool isCutting = false;
    Rigidbody2D rb;
    Camera c;
    CircleCollider2D circleCol;
    Vector2 previousPosition;
    public float minVel = .001f;
    public GameObject bladeTrailPrefab;
    GameObject currBladeTrail;

    //Panels and sounds
    public GameObject gameOverPanel;
    public GameObject gamePanel;
    public Text ScoreDisplay;
    public Text HighScoreDisplay;
    public int score;
    public AudioSource[] sounds;
    public AudioSource slice;
    public AudioSource bonus;
    public AudioSource cheer;

    private void Start()
    {
        c = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        circleCol = GetComponent<CircleCollider2D>();
        ScoreDisplay.text = score.ToString();               //display score
        sounds = GetComponents<AudioSource>();
        cheer = sounds[0];
        slice = sounds[1];
        bonus = sounds[2];
        HighScoreDisplay.text = PlayerPrefs.GetInt("Highscore", 0).ToString();          //get high score
    }
    // Update is called once per frame
    void Update()
    {
        // mouse button to make blade slice
        if (Input.GetMouseButtonDown(0))
        {
            StartCutting();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopCutting();
        }

        if (isCutting)
        {
            UpdateCutting();
        }
        if (PlayerPrefs.GetInt("Highscore", 0) == 0)
        {
            HighScoreDisplay.text = "0";
        }
    }

    void UpdateCutting()
    {
        Vector2 newPosition = c.ScreenToWorldPoint(Input.mousePosition);
        rb.position = newPosition;

        // Has to be faster than minimum velocity to register slice
        float velocity = (newPosition - previousPosition).magnitude * Time.deltaTime;
        if (velocity > minVel)
        {
            circleCol.enabled = true;
        }
        else
        {
            circleCol.enabled = false;
        }
        previousPosition = newPosition;
    }

    void StartCutting()
    {
        // Start trail and register slice
        isCutting = true;
        currBladeTrail = Instantiate(bladeTrailPrefab, transform);
        previousPosition = c.ScreenToWorldPoint(Input.mousePosition);
        circleCol.enabled = false;
    }

    void StopCutting()
    {
        isCutting = false;
        currBladeTrail.transform.SetParent(null);
        Destroy(currBladeTrail, 0f);
        circleCol.enabled = false;
    }

    // Check the tag and change the score accordingly 
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "logo")
        {
            score++;
            ScoreDisplay.text = score.ToString();
            slice.Play();
        }
        else if (col.tag == "knox")
        {
            ;
        }
        else
        {
            score += 10;
            ScoreDisplay.text = score.ToString(); 
            bonus.Play();
            slice.Play();
        }
            
        //Save player prefs 
        ScoreDisplay.text = score.ToString();
        if (score > PlayerPrefs.GetInt("Highscore", 0))
        {
            PlayerPrefs.SetInt("Highscore", score);
        }
        
    }

    public int getScore()
    {
        return score;
    }
}
