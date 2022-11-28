using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BulletBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update

    // bullets
    public GameObject blueStar;
    public GameObject greenStar;
    public GameObject redStar;
    public GameObject yellowStar;
    public GameObject pinkStar;
    public GameObject orangeStar;
    public GameObject simpleO;
    public GameObject littlePellet;

    // players and hearts and other objects
    public GameObject player1;
    public GameObject player2;

    public GameObject p1Heart1;
    public GameObject p1Heart2;
    public GameObject p1Heart3;

    public GameObject p2Heart1;
    public GameObject p2Heart2;
    public GameObject p2Heart3;

    public GameObject trainingText;

    public GameObject timer;


    // other critical parameters
    private Camera cam;
    public float pi = (float)Math.PI;

    public AudioSource deathSound;
    public static Boolean soundOn = true;

    public AudioSource backgroundMusic;
    public static Boolean musicOn = true;



    // reset game? reset all variables below AND destory all bullets tagged "fromPlayer1" and "fromPlayer2" AND reset position of players
    // and don't forget parameters in GameBehaviourScript

        // set to 300 when invincible
    public int p1InvincibleFrames = 0;
    public int p2InvincibleFrames = 0;

    public float varyingVelocity = 2;

    public Boolean playerIsFading = false;

    public float deltaFrame = 0.5F;


    public static int player1CardIndex = 0;
    public static int player2CardIndex = 0;

    public int p1TouchCount = 0;
    public int p2TouchCount = 0;

    public float frame = 0F;
    public int realFrame = 0;

    public GameObject[] bullets;

    public float hitBoxHalfWidth = 0.18F;

    delegate void SpellcardDelegate(GameObject Player, GameObject otherPlayer, int r, string Tag);

    public static int framesUntilStart = 42;

    public String timeString = "00:00";




    void Start()
    {
        cam = Camera.main;

        /*

        GameObject[] visibles = GameObject.FindGameObjectsWithTag("visible");

        foreach (GameObject visible in visibles)
        {
            setAlphaToCustom(visible, 1F);
        }

        */

    }



    // Update is called once per frame
    void Update()
    {
        if (GameBehaviourScript.needToResetValues)
        {
            resetGame();
            GameBehaviourScript.needToResetValues = false;
        }


        if (GameBehaviourScript.gameIsActive == true)
        {
            framesUntilStart--;

            realFrame++;

            // make game harder the longer it lasts
            // 2x speed after 3 mins (60 fps)

            frame += deltaFrame;

            deltaFrame = 0.5F + (frame / 10800);

            // Absolutely critical that that == 0. Otherwise it will be silent by playing over itself repeatedly
            if (framesUntilStart == 0)
            {
                if (musicOn)
                {
                    backgroundMusic.Play();
                    backgroundMusic.loop = true;

                }
            }

        } else
        {
            backgroundMusic.Stop();

        }

        // update timer if in training mode

        if (GameBehaviourScript.trainingMode && GameBehaviourScript.gameIsActive)
        {
            int seconds = (int) Math.Floor((double) realFrame / 60);

            int minutes = (int) Math.Floor((double) seconds / 60);

            seconds = seconds - (60 * minutes);

            String secondString = seconds.ToString();

            if (secondString.Length == 1)
            {
                secondString = "0" + secondString;
            }

            String minuteString = minutes.ToString();

            if (minuteString.Length == 1)
            {
                minuteString = "0" + minuteString;
            }

            timeString = minuteString + ":" + secondString;

            TextMeshPro timeText = timer.GetComponent<TextMeshPro>();

            timeText.SetText(timeString);
            timeText.color = new Color32(255, 255, 255, 255);


        }



        if (p1InvincibleFrames > 0)
        {
            p1InvincibleFrames--;

        }

        if (p2InvincibleFrames > 0)
        {
            p2InvincibleFrames--;
        }


        // #1) hit detection; square hitbox; removing hearts; playing death sound; announcing time on training mode


        if (realFrame > 240 && realFrame % 2 == 0)
        {
            List<GameObject> bulletList = new List<GameObject>();

            GameObject[] bulletsfromPlayer1 = GameObject.FindGameObjectsWithTag("fromPlayer1");
            GameObject[] bulletsfromPlayer2 = GameObject.FindGameObjectsWithTag("fromPlayer2");

            bulletList.AddRange(bulletsfromPlayer1);
            bulletList.AddRange(bulletsfromPlayer2);

            bullets = bulletList.ToArray();

            foreach (GameObject bullet in bullets)
            {


                if (p1InvincibleFrames <= 0 && bullet.tag == "fromPlayer2" &&
                      (Math.Abs(bullet.transform.position.x - player1.transform.position.x) < hitBoxHalfWidth &&
                       Math.Abs(bullet.transform.position.y - player1.transform.position.y) < hitBoxHalfWidth)
                   )
                {
                    

                    switch (GameBehaviourScript.p1LivesLeft)
                    {
                        case 3:
                            p1Heart1.tag = "toFade";
                            break;
                        case 2:
                            p1Heart2.tag = "toFade";
                            break;
                        case 1:
                            p1Heart3.tag = "toFade";
                            break;
                    }

                    GameBehaviourScript.p1LivesLeft--;

                    if (GameBehaviourScript.p1LivesLeft > 0)
                    {
                        fadeInAndOutPlayer(player1, GameBehaviourScript.p1LivesLeft);
                    } else
                    {
                        player1.tag = "toFade";

                        // TIME ANNOUNCEMENT
                        if (GameBehaviourScript.trainingMode)
                        {
                            TextMeshPro timeText = timer.GetComponent<TextMeshPro>();
                            RectTransform timeTextRectTransform = timer.GetComponent<RectTransform>();

                            timeText.SetText(timeString + "!");
                            timeTextRectTransform.position = new Vector3(0F, 0F, 0F);
                        }
                    }

                    playDeathSound(666);

                }
                    
                    
                // Training mode should make player 2 immune

                if ( GameBehaviourScript.trainingMode == false && p2InvincibleFrames <= 0 && bullet.tag == "fromPlayer1" &&
                      (Math.Abs(bullet.transform.position.x - player2.transform.position.x) < hitBoxHalfWidth &&
                       Math.Abs(bullet.transform.position.y - player2.transform.position.y) < hitBoxHalfWidth)
                   )
                {
                    
                    switch (GameBehaviourScript.p2LivesLeft)
                    {
                        case 3:
                            p2Heart1.tag = "toFade";
                            break;
                        case 2:
                            p2Heart2.tag = "toFade";
                            break;
                        case 1:
                            p2Heart3.tag = "toFade";
                            break;
                    }

                    GameBehaviourScript.p2LivesLeft--;

                    if (GameBehaviourScript.p2LivesLeft > 0)
                    {
                        fadeInAndOutPlayer(player2, GameBehaviourScript.p2LivesLeft);
                    } else
                    {
                        player2.tag = "toFade";
                    }

                    playDeathSound(666);

                }
            }

        }

        // #2) check for objects to fade

        GameObject[] objectsToFade = GameObject.FindGameObjectsWithTag("toFade");

        foreach (GameObject object2 in objectsToFade)
        {
            SpriteRenderer r = object2.GetComponent<SpriteRenderer>();
            Color newColor = r.material.color;

            if (newColor.a >= 0)
            {
                setAlphaToCustom(object2, newColor.a - 0.05F);
            } else
            {
                object2.tag = "invisible";
            }
            
        }

        // #3) check for players to fade in/out

        GameObject[] playersToFadeInAndOut = GameObject.FindGameObjectsWithTag("fadeInAndOut");

        foreach (GameObject player in playersToFadeInAndOut)
        {
            SpriteRenderer r = player.GetComponent<SpriteRenderer>();
            Color newColor = r.material.color;

            if ( (player == player1 && p1InvincibleFrames > 0) ||
                 (player == player2 && p2InvincibleFrames > 0)  ) 
            {

                if (newColor.a <= 0.1F)
                {
                    playerIsFading = false;

                } else if (newColor.a >= 0.6F) {

                    playerIsFading = true;

                }

                if (playerIsFading)
                {
                    setAlphaToCustom(player, newColor.a - 0.02F);
                }
                else
                {
                    setAlphaToCustom(player, newColor.a + 0.02F);
                }

            } else if (player == player1 && p2InvincibleFrames <= 0)
            {
                setAlphaToCustom(player, 1F);
                player.tag = "player1";

            } else if (player == player2 && p2InvincibleFrames <= 0)
            {
                setAlphaToCustom(player, 1F);
                player.tag = "player2";

            }

            

        }

        




        // #4) clean-up bullets every 8/60 seconds AND make player's own bullets less visble when near player

        if (realFrame % 8 == 0)
        {
            List<GameObject> bulletList = new List<GameObject>();

            GameObject[] bulletsfromPlayer1 = GameObject.FindGameObjectsWithTag("fromPlayer1");
            GameObject[] bulletsfromPlayer2 = GameObject.FindGameObjectsWithTag("fromPlayer2");

            bulletList.AddRange(bulletsfromPlayer1);
            bulletList.AddRange(bulletsfromPlayer2);

            bullets = bulletList.ToArray();

            // #5) bullets nearby player should be less visble if spawned by them


            foreach (GameObject bullet in bullets)
            {
                SpriteRenderer r = bullet.GetComponent<SpriteRenderer>();

                if ((bullet.transform.position.y < 0 && bullet.tag == "fromPlayer1") ||
                     (bullet.transform.position.y > 0 && bullet.tag == "fromPlayer2"))
                {
                    float k;

                    if (bullet.tag == "fromPlayer1")
                    {
                        k = (0.4F - 0.01F) * ((bullet.transform.position.y + 7) / (9.5F));
                    }
                    else
                    {
                        k = (0.4F - 0.01F) * ((7 - bullet.transform.position.y) / (9.5F));
                    }

                    if (k < 0)
                    {
                        k = 0;
                    }

                    
                    Color newColor = r.material.color;
                    newColor.a = 0.01F + k;
                    r.material.color = newColor;
                    
                }
                else
                {
                    Color newColor = r.material.color;
                    newColor.a = 1F;
                    r.material.color = newColor;
                }


            }

            foreach (GameObject bullet in bullets)
            {
                if (bullet.transform.position.x < -7 || bullet.transform.position.x > 7 ||
                    bullet.transform.position.y < -10 || bullet.transform.position.y > 10)
                {
                    Destroy(bullet);
                }
            }
        }

        // #6) change patterns by tapping; first touch should keep the same spellcard

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began && framesUntilStart <= 0)
            {
                Vector2 vector2 = cam.ScreenToWorldPoint(touch.position);

                if (vector2.y < 0)
                {
                    p1TouchCount++;
                    if (p1TouchCount > 1)
                    {
                        player1CardIndex += 1;
                    }
                }
                if (vector2.y > 0 && GameBehaviourScript.trainingMode == false)
                {
                    p2TouchCount++;
                    if (p2TouchCount > 1)
                    {
                        player2CardIndex += 1;
                    }
                }
            }
        }

        SpellcardDelegate[] ExecuteSpellcard = new SpellcardDelegate[]{ blueSpell, rainbowSpell, redGreenSpell, pinkSpell, greySpell };

        // #7) generate the bullets

        if (GameBehaviourScript.gameIsActive == true)
        {
            ExecuteSpellcard[player1CardIndex % 5](player1, player2, 1, "fromPlayer1");
            ExecuteSpellcard[player2CardIndex % 5](player2, player1, -1, "fromPlayer2");
            
        }
        


    }

    // 8
    void playDeathSound(int useless)
    {
        if (soundOn && GameBehaviourScript.gameIsActive)
        {
            deathSound.Play();
        }
    }

    // 9
    void createBullet(GameObject bulletOriginal, float posX, float posY, float velX, float velY, float accX, float accY, float spin, string tag)
    {
        // create bullet
        GameObject bulletClone = bulletOriginal;
        bulletClone.tag = tag;

        setAlphaTo0(bulletClone);

        GameObject gameBullet = Instantiate(bulletClone, new Vector2(posX, posY), Quaternion.identity);
        Rigidbody2D bulletbody2D = gameBullet.GetComponent<Rigidbody2D>();
        ConstantForce2D constantforce2D = gameBullet.GetComponent<ConstantForce2D>();

        // kinematics
        bulletbody2D.velocity = new Vector2(velX, velY);

        constantforce2D.force = new Vector2(accX, accY);
        constantforce2D.torque = spin;

    }

    // 10
    void createFollowingBullet(GameObject bulletOriginal, float posX, float posY, float followPosX, float followPosY, float vel, float acc, float spin, string tag)
    {
       
        float slopeToFollow = (followPosY - posY) / (followPosX - posX);

        double angleToFollow = Math.Atan(slopeToFollow);

        float velX = vel * (float) Math.Cos(angleToFollow);
        float velY = vel * (float) Math.Sin(angleToFollow);

        float accX = acc * (float)Math.Cos(angleToFollow);
        float accY = acc * (float)Math.Sin(angleToFollow);

        float[] values = new float[4] { velX, velY, accX, accY };

        for(int i = 0; i < 4; i++)
        {
            if (slopeToFollow < 0)
            {
                values[i] = -1 * values[i];
            }
            if (tag == "fromPlayer2")
            {
                values[i] = -1 * values[i];
            }
                
        }

        createBullet(bulletOriginal, posX, posY, (float) values.GetValue(0), (float) values.GetValue(1), 
            (float) values.GetValue(2), (float) values.GetValue(3), spin, tag);


    }

    // 11
    void createBulletByAngle(GameObject bulletOriginal, float posX, float posY, float angle, float turn, float turnAngle, float vel, float spin, string tag)
    {

        // determine velocities to create angle
        float velX = vel * (float)Math.Cos(angle);

        float velY = vel * (float)Math.Sin(angle);

        // turning mechanism
        float accX = turn * (float)Math.Cos(angle + turnAngle);

        float accY = turn * (float)Math.Sin(angle + turnAngle);

        createBullet(bulletOriginal, posX, posY, velX, velY, accX, accY, spin, tag);

    }


    // 12
    public void setAlphaTo0(GameObject gameObject)
    {
        SpriteRenderer r = gameObject.GetComponent<SpriteRenderer>();
        Color newColor = r.material.color;
        newColor.a = 0F;
        r.material.color = newColor;
    }

    public void setAlphaToCustom(GameObject gameObject, float alpha)
    {
        SpriteRenderer r = gameObject.GetComponent<SpriteRenderer>();
        Color newColor = r.material.color;
        newColor.a = alpha;
        r.material.color = newColor;
    }

    // 13
    void fadeInAndOutPlayer(GameObject player, int LivesLeft)
    {

        // keep player invincible temporarily

        if (player == player1)
        {
            p1InvincibleFrames = 300;
        }
        else if (player == player2)
        {
            p2InvincibleFrames = 300;
        }

        setAlphaTo0(player);

        // only reset body if there are > 0 lives left
        
        player.tag = "fadeInAndOut";


    }

    // 14

    // This is where all the beautiful bullet patterns originate
    // int r reverses y-direction for player 2 (and sometimes also position)

    // Future update: players can collect spellcards, allowing for customization

    // current available spellcards: blueSpell(), rainbowSpell(), redGreenSpell(), pinkSpell(), greySpell()

    void blueSpell(GameObject Player, GameObject otherPlayer, int r, string Tag)
    {
        // basic pattern: shooting widespread, slowish
        if (frame % 2 < deltaFrame)
        {
            createBullet(blueStar, Player.transform.position.x, Player.transform.position.y, ((frame % 11) - 5.5F) * 0.8F + ((frame % 367 - 183.5F) / 180), r * 3, 0, 0, 100, Tag);
        }
    }
    void rainbowSpell(GameObject Player, GameObject otherPlayer, int r, string Tag)
    {

        // spread rainbow pattern
        // spread variable
        float k = 0.9F;

        // angling (acceleration) variable
        float m = 0.6F;

        float velY = 7F;

        if (frame % 48 < deltaFrame)
        {
            createBullet(blueStar, Player.transform.position.x - (9 * k), Player.transform.position.y, 0, r * (velY + 2), (5 * m), 0, 100, Tag);

            createBullet(pinkStar, Player.transform.position.x - (7 * k), Player.transform.position.y, 0, r * (velY + 1.5F), (4 * m), 0, 100, Tag);

            createBullet(redStar, Player.transform.position.x - (5 * k), Player.transform.position.y, 0, r * (velY + 1), (3 * m), 0, 100, Tag);

            createBullet(orangeStar, Player.transform.position.x - (3 * k), Player.transform.position.y, 0, r * (velY + 0.5F), (2 * m), 0, 100, Tag);

            createBullet(yellowStar, Player.transform.position.x - k, Player.transform.position.y, 0, r * velY, m, 0, 100, Tag);

            createBullet(greenStar, Player.transform.position.x + k, Player.transform.position.y, 0, r * velY, -m, 0, 100, Tag);

            createBullet(blueStar, Player.transform.position.x + (3 * k), Player.transform.position.y, 0, r * (velY + 0.5F), (-2 * m), 0, 100, Tag);

            createBullet(pinkStar, Player.transform.position.x + (5 * k), Player.transform.position.y, 0, r * (velY + 1), (-3 * m), 0, 100, Tag);

            createBullet(redStar, Player.transform.position.x + (7 * k), Player.transform.position.y, 0, r * (velY + 1.5F), (-4 * m), 0, 100, Tag);

            createBullet(orangeStar, Player.transform.position.x + (9 * k), Player.transform.position.y, 0, r * (velY + 2), (-5 * m), 0, 100, Tag);

        }
    }
    void redGreenSpell(GameObject Player, GameObject otherPlayer, int r, string Tag)
    {
        
        // bullets follow player, turning circle shots of 17
        if (frame % 10 < deltaFrame)
        {
            createFollowingBullet(greenStar, -4, r * (-9), otherPlayer.transform.position.x, otherPlayer.transform.position.y, 7, 1, 100, Tag);
            createFollowingBullet(greenStar, 4, r * (-9), otherPlayer.transform.position.x, otherPlayer.transform.position.y, 7, 1, 100, Tag);
        }
        if (frame % 30 < deltaFrame)
        {
            for (int i = 0; i < 17; i++)
            {
                createBulletByAngle(redStar, Player.transform.position.x, Player.transform.position.y, (i * 2 * pi / 17) + frame, 5, (90 * pi / 180), 5, 100, Tag);
            }
        }
    }
    void pinkSpell(GameObject Player, GameObject otherPlayer, int r, string Tag)
    {
        
        // a pink swirl of varying direction of acceleration
        if (frame % 7 < deltaFrame)
        {
            for (int i = 0; i < 13; i++)
            {
                createBulletByAngle(pinkStar, Player.transform.position.x, Player.transform.position.y, (i * 2 * pi / 13) + ((frame / 7000) * 2 * pi), 3, (frame / 1.8F * pi / 180), 1.2F, 100, Tag);
            }
        }
    }

    void greySpell(GameObject Player, GameObject otherPlayer, int r, string Tag)
    {

        // NEW idea: small pellets that cycle between slow and getting faster (to the point where the fast bullets overtake the slow ones)
        //           And use the large bullets too
        if (frame % 40 < deltaFrame)
        {
            for (int i = 0; i < 27; i++)
            {
                createBulletByAngle(simpleO, Player.transform.position.x, Player.transform.position.y, (i * 2 * pi / 27) + (frame * 1.618F), 0, 0, 3F, 0, Tag);
            }
        }

        float incrementVelocityBy = 0.243F;

        float incrementAngleBy = 1.212121F * pi / 180;

        varyingVelocity = varyingVelocity + incrementVelocityBy;

        if (varyingVelocity >= 10)
        {
            varyingVelocity = 2;
        }

        if (frame % 5 < deltaFrame)
        {
            
            for (int i = 0; i < 13; i++)
            {
                createBulletByAngle(littlePellet, Player.transform.position.x, Player.transform.position.y, (i * 2 * pi / 13) + (frame * incrementAngleBy), 0, 0, varyingVelocity, 0, Tag);
            }
        }
        
        
    }

    /*  // old version
    
    void greySpell(GameObject Player, GameObject otherPlayer, int r, string Tag)
    {
        
        // simple straight, but deadly shot

        if (frame % 11 < deltaFrame)
        {
            createBullet(littlePellet, Player.transform.position.x, Player.transform.position.y, 0, r * 10, 0, 0, 0, Tag);
        }
    }

    */

    // 15

    

    void resetGame()
    {
        p1InvincibleFrames = 0;
        p2InvincibleFrames = 0;

        varyingVelocity = 2;

        playerIsFading = false;

        deltaFrame = 0.5F;

        p1TouchCount = 0;
        p2TouchCount = 0;

        // GameObject[] bullets = new GameObject[];

        // card indexes left out for good reason

        hitBoxHalfWidth = 0.18F;

        cam = Camera.main;
        realFrame = 0;
        frame = 0F;

        GameBehaviourScript.p1LivesLeft = 3;

        GameBehaviourScript.p2LivesLeft = 3;

        setAlphaToCustom(player1, 1F);
        setAlphaToCustom(player2, 1F);

        player1.transform.position = new Vector3(0F, -8.4F, 0F);

        player2.transform.position = new Vector3(0F, 8.4F, 0F);

        GameObject[] faders = GameObject.FindGameObjectsWithTag("toFade");

        foreach (GameObject fader in faders)
        {
            fader.tag = "visible";
        }

        setAlphaToCustom(p1Heart1, 1F); setAlphaToCustom(p1Heart2, 1F); setAlphaToCustom(p1Heart3, 1F);

        if (GameBehaviourScript.trainingMode == false)
        {
            setAlphaToCustom(p2Heart1, 1F); setAlphaToCustom(p2Heart2, 1F); setAlphaToCustom(p2Heart3, 1F);
            setAlphaToCustom(trainingText, 0F);

            TextMeshPro timeText = timer.GetComponent<TextMeshPro>();
            timeText.color = new Color32(255, 255, 255, 0);
            

        } else
        {
            setAlphaToCustom(p2Heart1, 0F); setAlphaToCustom(p2Heart2, 0F); setAlphaToCustom(p2Heart3, 0F);
            setAlphaToCustom(trainingText, 1F);

            TextMeshPro timeText = timer.GetComponent<TextMeshPro>();
            timeText.color = new Color32(255, 255, 255, 255);
            
        }

        framesUntilStart = 42;

        GameBehaviourScript.p1LivesLeft = 3;

        GameBehaviourScript.p2LivesLeft = 3;

        TextMeshPro timeText2 = timer.GetComponent<TextMeshPro>();
        
        timeText2.SetText("00:00");

        RectTransform timeTextRectTransform = timer.GetComponent<RectTransform>();

        timeTextRectTransform.position = new Vector3(-3.61F, 8.56F, 0F);





    }



}
