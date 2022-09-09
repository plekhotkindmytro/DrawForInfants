using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputHandler : MonoBehaviour
{
    GameObject particle;


    // Update is called once per frame
    void Update()
    {
        countFingers();
        fireRay();
        Accelerate();
        GetAccelerometerValue();

        lowPassValue = LowPassFilterAccelerometer(lowPassValue);

    }
    /*
     * 
     * TouchPhase.Began - A user has touched their finger to the screen this frame
     * TouchPhase.Stationary - A finger is on the screen but the user has not moved it this frame
     * TouchPhase.Moved - A user moved their finger this frame
     * TouchPhase.Ended - A user lifted their finger from the screen this frame
     * TouchPhase.Cancelled - The touch was interrupted this frame
     *
     */
    private void countFingers()
    {
        var fingerCount = 0;
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
            {
                fingerCount++;
            }
        }
        if (fingerCount > 0)
        {
            print("User has " + fingerCount + " finger(s) touching the screen");
        }
    }

    private void fireRay() 
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                // Construct a ray from the current touch coordinates
                
                Vector2 worldPoint = Camera.main.ScreenToWorldPoint(touch.position);
                if (Physics2D.Raycast(worldPoint, Vector2.zero))
                {
                    // Create a particle if hit
                    Instantiate(particle, transform.position, transform.rotation);
                }
            }
        }
    }

    

    private void Accelerate()
    {
        float speed = 10.0f;
        Vector3 dir = Vector3.zero;
        // we assume that the device is held parallel to the ground
        // and the Home button is in the right hand

        // remap the device acceleration axis to game coordinates:
        // 1) XY plane of the device is mapped onto XZ plane
        // 2) rotated 90 degrees around Y axis

        dir.x = -Input.acceleration.y;
        dir.z = Input.acceleration.x;

        // clamp acceleration vector to the unit sphere
        if (dir.sqrMagnitude > 1)
            dir.Normalize();

        // Make it move 10 meters per second instead of 10 meters per frame...
        dir *= Time.deltaTime;

        // Move object
        transform.Translate(dir * speed);
    }

    Vector3 GetAccelerometerValue()
    {
        Vector3 acc = Vector3.zero;
        float period = 0.0f;

        foreach (AccelerationEvent evnt in Input.accelerationEvents)
        {
            acc += evnt.acceleration * evnt.deltaTime;
            period += evnt.deltaTime;
        }
        if (period > 0)
        {
            acc *= 1.0f / period;
        }
        return acc;
    }


    // Low Pass filter for accelerometer 
    float accelerometerUpdateInterval = 1.0f / 60.0f;
    float lowPassKernelWidthInSeconds = 1.0f;

    private float lowPassFilterFactor;
    private Vector3 lowPassValue = Vector3.zero;

    void Start()
    {
        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        lowPassValue = Input.acceleration;
    }

    Vector3 LowPassFilterAccelerometer(Vector3 prevValue)
    {
        Vector3 newValue = Vector3.Lerp(prevValue, Input.acceleration, lowPassFilterFactor);
        return newValue;
    }

}
