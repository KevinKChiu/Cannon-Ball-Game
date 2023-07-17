using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

// change this class as you may see fit
public class ParticleMotion : MonoBehaviour
{
    float[] particle_state;
    Vector3 lcenter, rcenter;

    // Particle (ball instance initialization)
    void Start()
    {        
        particle_state = new float[6]; // you may keep the velocity/position in the same array (it can also be separated into two Vector3's)
        lcenter = new Vector3(-5.0f, 1.0f, 0.0f);  // left center of gravity 
        rcenter = new Vector3(5.0f, 1.0f, 0.0f);  // right center of gravity 

        // initialize particle state (position from the initial instance's position, and velocity according to the cannon orientation)
        for (int d = 0; d < 3; d++)
            particle_state[d] = transform.position[d];  

        float angle_radians = transform.rotation.eulerAngles.x * (Mathf.PI / 180.0f);
        particle_state[3] = 2.0f * Mathf.Cos(-angle_radians);
        particle_state[4] = 2.0f * Mathf.Sin(-angle_radians);
        particle_state[5] = 0.0f;
    }

    void FixedUpdate()
    {
        // your function to implement!
        UpdateState(lcenter, rcenter, Time.deltaTime, ref particle_state);
        
        // update position for particle
        transform.position = new Vector3(particle_state[0], particle_state[1], particle_state[2]);
    }



    // you need to change this function for numerical integration to calculate the particle state
    // to support
    // (a) two attraction forces
    // (b) a simple linear drag function (for good balls)
    // (c) update velocity from forces, and
    // (d) update position from velocity
    void UpdateState(Vector3 lcenter, Vector3 rcenter, float dt, ref float[] particle_state)
    {
		// delete these lines and update the particle state based on the Newton's law of motion and  forces described above
        // for (int d = 0; d < 3; d++)
        //     particle_state[d] += particle_state[d + 3] * dt;

        float m = 0.18f;
        Vector3 position = new Vector3(particle_state[0], particle_state[1], particle_state[2]);
        Vector3 velocity = new Vector3(particle_state[3], particle_state[4], particle_state[5]);

        Vector3 f1a = (rcenter - position) / (rcenter - position).magnitude;  
        Vector3 f1b = (lcenter - position) / (lcenter - position).magnitude;
        Vector3 f2 = -0.012f * velocity;

        for (int d = 0; d < 3; d++) {
            if (name.Contains("cannon_ball_template_good_clone")) {
                particle_state[d + 3] +=  dt * ((f1a[d] + f1b[d] + f2[d]) / m);
            } else {
                particle_state[d + 3] += dt * ((f1a[d] + f1b[d]) / m);
            }
            particle_state[d] += dt * particle_state[d + 3];
        }
    }
}
