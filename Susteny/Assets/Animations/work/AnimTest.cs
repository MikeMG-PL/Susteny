using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimTest : MonoBehaviour
{
    public List<GameObject> characters;
    public GameObject cube;

    void Start()
    {
        if (characters.Count > 0)
        {
            foreach (GameObject o in characters)
            {
                AnimationSystem.animate(o, Animations.IDLE_1, AnimationBodyPart.WHOLE_BODY);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            AnimationSystem.startLookingAt(characters[0], cube);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            AnimationSystem.stopLookingAtAll(characters[0]);

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            // Zróbmy tutaj coś fajnego

            AnimationSystem.animate(characters[1], Animations.TALKING_2, AnimationBodyPart.UPPER_BODY);
            AnimationSystem.animate(characters[1], Animations.SITTING_1, AnimationBodyPart.LOWER_BODY);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
            AnimationSystem.stopAnimation(characters[1]);
    }
}
