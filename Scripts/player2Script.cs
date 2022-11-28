using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class player2Script : MonoBehaviour
{


    public float speed = 10;

    private Camera cam;
    
    public Vector2 vector2;

    public int frame = 0;

    public Vector3 nextPoint;

    public System.Random rand;
    public float randomX;
    public float randomY;
    public float randomVelocity;
    public float randomWaitingFrames;
    public int randomSpellcardDuration;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        rand = new System.Random();

        randomX = (float)(rand.NextDouble() - 0.5) * 10;
        randomY = (float)rand.NextDouble() * 9;
        randomVelocity = (float)(0.5 * rand.NextDouble()) + 0.1F;
        randomWaitingFrames = rand.Next(10, 120);
        randomSpellcardDuration = rand.Next(7, 31) * rand.Next(7, 31);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameBehaviourScript.gameIsActive == true && BulletBehaviourScript.framesUntilStart <= 0)
        {

            if (GameBehaviourScript.trainingMode == true)
            {

                frame++;

                nextPoint = new Vector3(randomX, randomY, 0);

                transform.position = Vector3.MoveTowards(transform.position, nextPoint, randomVelocity);
                if (transform.position == nextPoint)
                {
                    randomWaitingFrames--;
                    if (randomWaitingFrames <= 0)
                    {
                        randomX = (float)(rand.NextDouble() - 0.5) * 10;
                        randomY = (float)rand.NextDouble() * 9;
                        randomVelocity = (float)(0.5 * rand.NextDouble()) + 0.1F;
                        randomWaitingFrames = rand.Next(10, 120);
                    }
                }


                if (frame % randomSpellcardDuration == 0)
                {
                    randomSpellcardDuration = rand.Next(7, 31) * rand.Next(7, 31);
                    BulletBehaviourScript.player2CardIndex++;
                }



            }
            else
            {


                foreach (Touch touch in Input.touches)
                {
                    if (Input.touchCount > 1)
                    {
                        // move the player towards where finger was moved

                        var firstTouch = Input.GetTouch(0);
                        var secondTouch = Input.GetTouch(1);

                        Vector2 firstVector2 = cam.ScreenToWorldPoint(firstTouch.position);

                        Vector2 secondVector2 = cam.ScreenToWorldPoint(secondTouch.position);

                        if (firstVector2.y > 0)
                        {
                            vector2 = firstVector2;
                        }
                        else
                        {
                            vector2 = secondVector2;
                        }


                    }
                    else
                    {
                        // move the player towards where finger was moved

                        vector2 = cam.ScreenToWorldPoint(touch.position);

                    }

                    vector2.y -= 1.3F;
                }

                Vector3 newPositionVector3 = vector2;

                float step = speed;

                // player 1 uses bottom half of screen, player 2 uses top half (19 is the total height, 0 is the center)

                if (newPositionVector3.y > 0)
                {
                    transform.position = Vector3.MoveTowards(transform.position, newPositionVector3, step);

                }

            }


        }

    }
        
}