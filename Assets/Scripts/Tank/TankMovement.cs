using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;         


    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
    

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
        this.m_MovementInputValue = Input.GetAxis(this.m_MovementAxisName);
        this.m_TurnInputValue = Input.GetAxis(this.m_TurnAxisName);

        this.EngineAudio();
    }


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
        // If there is no movement (input) (the tank is stationary)...
        if (Mathf.Abs(this.m_MovementInputValue) < 0.1f && Mathf.Abs(this.m_TurnInputValue) < 0.1f)
        {
            // ... and if the audio source is currently playing the driving clip...
            if (this.m_MovementAudio.clip == this.m_EngineDriving)
            {
                // ... change the clip to idling and play it.
                this.m_MovementAudio.clip = this.m_EngineIdling;
                this.m_MovementAudio.pitch = Random.Range(this.m_OriginalPitch - this.m_PitchRange, this.m_OriginalPitch + this.m_PitchRange);
                this.m_MovementAudio.Play();
            }
        }
        else
        {
            // Otherwise if the tank is moving and if the idling clip is currently playing...
            if (this.m_MovementAudio.clip == this.m_EngineIdling)
            {
                // ... change the clip to driving and play.
                this.m_MovementAudio.clip = this.m_EngineDriving;
                this.m_MovementAudio.pitch = Random.Range(this.m_OriginalPitch - this.m_PitchRange, this.m_OriginalPitch + this.m_PitchRange);
                this.m_MovementAudio.Play();
            }
        }
    }


    private void FixedUpdate()
    {
        // Move and turn the tank.
        Move();
        Turn();
    }


    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * this.m_MovementInputValue * this.m_Speed * Time.deltaTime;

        // Apply this movement to the rigidbody's position.
        this.m_Rigidbody.MovePosition(this.m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = this.m_TurnInputValue * this.m_TurnSpeed * Time.deltaTime;

        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

        // Apply this rotation to the rigidbody's rotation.
        this.m_Rigidbody.MoveRotation(this.m_Rigidbody.rotation * turnRotation);
    }
}