using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBehaviourScript : MonoBehaviour
{

    // IF YOU WANT SOMETHING TO APPEAR, DO NOT CHANGE ALPHA IN UNITY EDITOR FROM 255

    public GameObject p1SpellcardBar;
    public GameObject p2SpellcardBar;

    public GameObject p1Card1;
    public GameObject p1Card2;
    public GameObject p1Card3;
    public GameObject p1Card4;
    public GameObject p1Card5;

    public GameObject p2Card1;
    public GameObject p2Card2;
    public GameObject p2Card3;
    public GameObject p2Card4;
    public GameObject p2Card5;

    public GameObject[] allTheSpellcards;

    public static int p1LivesLeft = 3;
    public static int p2LivesLeft = 3;

    private Camera cam;


    // Game settings
    public static Boolean gameIsActive = false;

    public static Boolean menuIsActive = true;

    public static Boolean trainingMode = true;

    public Boolean endCardIsActive = false;

    public Boolean instructionsAreActive = false;

    public static Boolean needToResetValues = false;


    // End cards
    public GameObject greenBotEndCard;
    public GameObject blueBotEndCard;
    public GameObject survivalEndCard;

    public static GameObject currentEndCard;

    // Menu, menu objects
    public GameObject menu;

    public GameObject musicMark;
    public GameObject soundFXmark;

    public GameObject instructions;

    public Boolean hasTouchedInstructions = false;

    public AudioSource menuMusic;


    // Button coordinates
    public float[] startGameButtonCoor;
    public float[] trainingModeButtonCoor;
    public float[] howToPlayButtonCoor;
    public float[] musicOnButtonCoor;
    public float[] soundFXOnButtonCoor;

    public float[] yesButtonCoor;
    public float[] backToMenuButtonCoor;


    // Start is called before the first frame update
    void Start()
    {
        allTheSpellcards = new GameObject[10]{ p1Card1, p1Card2, p1Card3, p1Card4, p1Card5,
                                               p2Card1, p2Card2, p2Card3, p2Card4, p2Card5, };

        menuMusic.Play();
        menuMusic.loop = true;

        cam = Camera.main;

        // #1 set button coordinates (minX, maxX, minY, maxY)

        startGameButtonCoor = new float[4] { -3.17F, 2.81F, 0.28F, 1.96F };

        trainingModeButtonCoor = new float[4] { -3.21F, 2.78F, -2.72F, -0.99F };

        howToPlayButtonCoor = new float[4] { -0.57F, 4.1F, -4.75F, -3.66F };

        musicOnButtonCoor = new float[4] { 2.43F, 3.66F, -6.81F, -5.62F };

        soundFXOnButtonCoor = new float[4] { 2.42F, 3.66F, -8.64F, -7.42F };

        yesButtonCoor = new float[4] { -2.97F, -0.52F, -5.95F, -4.82F };

        backToMenuButtonCoor = new float[4] { 0.62F, 3.06F, -5.96F, -4.83F };




        // #2 dim the spellcard bars, keep spells invisible at start

        setAlphaToCustom(p1SpellcardBar, 0.7F);
        setAlphaToCustom(p2SpellcardBar, 0.7F);

        GameObject[] spellcards = GameObject.FindGameObjectsWithTag("spellcard");

        foreach (GameObject spellcard in allTheSpellcards)
        {
            setAlphaToCustom(spellcard, 0F);
        }

        // hide certain items

        setAlphaToCustom(instructions, 0F);
        setAlphaToCustom(greenBotEndCard, 0F);
        setAlphaToCustom(blueBotEndCard, 0F);
        setAlphaToCustom(survivalEndCard, 0F);

    }

    // Update is called once per frame
    void Update()
    {
        // #3 Manage the menu here
        if (menuIsActive)
        {
            toggleVisibility(menu, true);
            toggleVisibility(musicMark, BulletBehaviourScript.musicOn);
            toggleVisibility(soundFXmark, BulletBehaviourScript.soundOn);

            hasTouchedInstructions = false;



            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began && !instructionsAreActive)
                {
                    Vector2 touchVector2 = cam.ScreenToWorldPoint(touch.position);

                    // start game
                    if (touchVector2.x > startGameButtonCoor[0] && touchVector2.x < startGameButtonCoor[1] &&
                        touchVector2.y > startGameButtonCoor[2] && touchVector2.y < startGameButtonCoor[3])
                    {
                        trainingMode = false;

                        toggleVisibility(menu, false);
                        toggleVisibility(musicMark, false);
                        toggleVisibility(soundFXmark, false);

                        menuMusic.Stop();
                        gameIsActive = true;
                        needToResetValues = true;
                        menuIsActive = false;
                    }

                    // training mode button
                    if (touchVector2.x > trainingModeButtonCoor[0] && touchVector2.x < trainingModeButtonCoor[1] &&
                        touchVector2.y > trainingModeButtonCoor[2] && touchVector2.y < trainingModeButtonCoor[3])
                    {
                        trainingMode = true;

                        toggleVisibility(menu, false);
                        toggleVisibility(musicMark, false);
                        toggleVisibility(soundFXmark, false);

                        menuMusic.Stop();
                        gameIsActive = true;
                        needToResetValues = true;
                        menuIsActive = false;
                    }

                    // how to play button
                    if (touchVector2.x > howToPlayButtonCoor[0] && touchVector2.x < howToPlayButtonCoor[1] &&
                        touchVector2.y > howToPlayButtonCoor[2] && touchVector2.y < howToPlayButtonCoor[3])
                    {
                        toggleVisibility(instructions, true);

                        instructionsAreActive = true;
                        menuIsActive = false;


                    }

                    // toggle music on/off
                    if (touchVector2.x > musicOnButtonCoor[0] && touchVector2.x < musicOnButtonCoor[1] &&
                        touchVector2.y > musicOnButtonCoor[2] && touchVector2.y < musicOnButtonCoor[3])
                    {
                        BulletBehaviourScript.musicOn = !BulletBehaviourScript.musicOn;

                        toggleVisibility(musicMark, BulletBehaviourScript.musicOn);

                        if (BulletBehaviourScript.musicOn)
                        {
                            menuMusic.Play();
                        }
                        else
                        {
                            menuMusic.Stop();

                        }

                        
                    }

                    // toggle sound on/off
                    if (touchVector2.x > soundFXOnButtonCoor[0] && touchVector2.x < soundFXOnButtonCoor[1] &&
                        touchVector2.y > soundFXOnButtonCoor[2] && touchVector2.y < soundFXOnButtonCoor[3])
                    {
                        BulletBehaviourScript.soundOn = !BulletBehaviourScript.soundOn;

                        toggleVisibility(soundFXmark, BulletBehaviourScript.soundOn);
                    }
                }
                    

            }

        }


        // We need hasTouchedInstructions because the touch that opens instructions also closes it
        if (instructionsAreActive)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (hasTouchedInstructions)
                    {
                        toggleVisibility(instructions, false);
                        menuIsActive = true;
                        instructionsAreActive = false;

                        
                    }
                    hasTouchedInstructions = true;
                    
                    
                }
                
            }

        }



        // #4 Manage the end cards here
        if (endCardIsActive)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector2 touchVector2 = cam.ScreenToWorldPoint(touch.position);

                    // yes button, plays again
                    if (touchVector2.x > yesButtonCoor[0] && touchVector2.x < yesButtonCoor[1] &&
                        touchVector2.y > yesButtonCoor[2] && touchVector2.y < yesButtonCoor[3])
                    {
                        gameIsActive = true;
                        needToResetValues = true;

                        setAlphaToCustom(currentEndCard, 0F);
                        endCardIsActive = false;
                    }

                    // back to menu button
                    if (touchVector2.x > backToMenuButtonCoor[0] && touchVector2.x < backToMenuButtonCoor[1] &&
                        touchVector2.y > backToMenuButtonCoor[2] && touchVector2.y < backToMenuButtonCoor[3])
                    {
                        menuIsActive = true;
                        needToResetValues = true;

                        if (BulletBehaviourScript.musicOn)
                        {
                            menuMusic.Play();
                        }

                        gameIsActive = false;

                        setAlphaToCustom(currentEndCard, 0F);
                        endCardIsActive = false;
                    }
                }
                    

            }

        }

        // #5 make sure the selected spellcard is glowing

        switch (BulletBehaviourScript.player1CardIndex % 5)
        {
            case 0:
                p1Card1.tag = "cardToHighlight"; p1Card5.tag = "cardToFade";
                break;
            case 1:
                p1Card2.tag = "cardToHighlight"; p1Card1.tag = "cardToFade";
                break;
            case 2:
                p1Card3.tag = "cardToHighlight"; p1Card2.tag = "cardToFade";
                break;
            case 3:
                p1Card4.tag = "cardToHighlight"; p1Card3.tag = "cardToFade";
                break;
            case 4:
                p1Card5.tag = "cardToHighlight"; p1Card4.tag = "cardToFade";
                break;
        }

        switch (BulletBehaviourScript.player2CardIndex % 5)
        {
            case 0:
                p2Card1.tag = "cardToHighlight"; p2Card5.tag = "cardToFade";
                break;
            case 1:
                p2Card2.tag = "cardToHighlight"; p2Card1.tag = "cardToFade";
                break;
            case 2:
                p2Card3.tag = "cardToHighlight"; p2Card2.tag = "cardToFade";
                break;
            case 3:
                p2Card4.tag = "cardToHighlight"; p2Card3.tag = "cardToFade";
                break;
            case 4:
                p2Card5.tag = "cardToHighlight"; p2Card4.tag = "cardToFade";
                break;
        }


        if (p1LivesLeft <= 0 && gameIsActive)
        {
            gameIsActive = false;
            p1LivesLeft = 3;

            if (trainingMode == false)
            {
                announceWinner("Green bot");
            }
            else
            {
                announceWinner("Training mode");
            }
        }
        if (p2LivesLeft <= 0 && gameIsActive)
        {
            gameIsActive = false;
            p2LivesLeft = 3;

            if (trainingMode == false)
            {
                announceWinner("Ice blue bot");
            }

        }

        // #6 fading and highlighting cards, also fades and highlights end cards and menu items

        GameObject[] cardsToFade = GameObject.FindGameObjectsWithTag("cardToFade");

        foreach (GameObject card in cardsToFade)
        {
            SpriteRenderer r = card.GetComponent<SpriteRenderer>();
            Color newColor = r.material.color;

            if (newColor.a > 0F)
            {
                setAlphaToCustom(card, newColor.a - 0.1F);
            }
            else
            {
                card.tag = "spellcard";
            }

        }

        GameObject[] cardsToHighlight = GameObject.FindGameObjectsWithTag("cardToHighlight");

        foreach (GameObject card in cardsToHighlight)
        {
            SpriteRenderer r = card.GetComponent<SpriteRenderer>();
            Color newColor = r.material.color;

            if (newColor.a < 1)
            {
                setAlphaToCustom(card, newColor.a + 0.1F);
            }
            else
            {
                card.tag = "spellcard";
            }

        }

    }

    void toggleVisibility(GameObject toggleObject, Boolean visible)
    {
        if (visible)
        {
            toggleObject.tag = "cardToHighlight";

        }
        else
        {
            toggleObject.tag = "cardToFade";
        }
    }

    void announceWinner(string winner)
    {

        endCardIsActive = true;


        // #7 End game card

        switch (winner) 
        {
            case "Green bot":
                greenBotEndCard.tag = "cardToHighlight";
                currentEndCard = greenBotEndCard;

                break;
            case "Ice blue bot":
                blueBotEndCard.tag = "cardToHighlight";
                currentEndCard = blueBotEndCard;

                break;
            case "Training mode":
                survivalEndCard.tag = "cardToHighlight";
                currentEndCard = survivalEndCard;

                break;
        }


    }

    void setAlphaToCustom(GameObject gameObject, float alpha)
    {
        SpriteRenderer r = gameObject.GetComponent<SpriteRenderer>();
        Color newColor = r.material.color;
        newColor.a = alpha;
        r.material.color = newColor;
    }
}
