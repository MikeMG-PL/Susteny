using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_HeadBobber : MonoBehaviour
{
    public float walkingBobbingSpeed = 14f;
    public float bobbingAmountWalk = 0.05f;
    public float bobbingAmountSprint = 0.1f;
    public SC_FPSController controller;

    float defaultPosY = 0;
    float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        float bobbingAmount = controller.isRunning ? bobbingAmountSprint : bobbingAmountWalk;
        float bobbingSpeed = controller.isRunning ? 18f : walkingBobbingSpeed;
        if (Mathf.Abs(controller.moveDirection.x) > 0.1f || Mathf.Abs(controller.moveDirection.z) > 0.1f)
        {
            //Player is moving
            timer += Time.deltaTime * bobbingSpeed;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount, transform.localPosition.z);
        }
        else
        {
            //Idle
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * bobbingSpeed), transform.localPosition.z);
        }
    }
}