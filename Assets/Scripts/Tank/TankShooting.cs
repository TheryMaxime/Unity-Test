using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;
    public Rigidbody m_Shell;
    public Transform m_FireTransform;
    public Slider m_AimSlider;
    public AudioSource m_ShootingAudio;
    public AudioClip m_ChargingClip;
    public AudioClip m_FireClip;
    public float m_MinLaunchForce = 15f;
    public float m_MaxLaunchForce = 30f;
    public float m_MaxChargeTime = 0.75f;


    private string m_FireButton;
    private float m_CurrentLaunchForce;
    private float m_ChargeSpeed;
    private bool m_Fired;


    private void OnEnable()
    {
        this.m_CurrentLaunchForce = this.m_MinLaunchForce;
        this.m_AimSlider.value = this.m_MinLaunchForce;
    }


    private void Start()
    {
        this.m_FireButton = "Fire" + this.m_PlayerNumber;

        this.m_ChargeSpeed = (this.m_MaxLaunchForce - this.m_MinLaunchForce) / this.m_MaxChargeTime;
    }


    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        // The slider should have a default value of the minimum launch force.
        this.m_AimSlider.value = this.m_MinLaunchForce;

        // If the max force has been exceeded and the shell hasn't yet been launched...
        if (this.m_CurrentLaunchForce >= this.m_MaxLaunchForce && !this.m_Fired)
        {
            // ... use the max force and launch the shell.
            this.m_CurrentLaunchForce = this.m_MaxLaunchForce;
            this.Fire();
        }
        // Otherwise, if the fire button has just started being pressed...
        else if (Input.GetButtonDown(this.m_FireButton))
        {
            // ... reset the fired flag and reset the launch force.
            this.m_Fired = false;
            this.m_CurrentLaunchForce = this.m_MinLaunchForce;

            // Change the clip to the charging clip and start it playing.
            this.m_ShootingAudio.clip = this.m_ChargingClip;
            this.m_ShootingAudio.Play();
        }
        // Otherwise, if the fire button is being held and the shell hasn't been launched yet...
        else if (Input.GetButton(this.m_FireButton) && !this.m_Fired)
        {
            // Increment the launch force and update the slider.
            this.m_CurrentLaunchForce += this.m_ChargeSpeed * Time.deltaTime;

            this.m_AimSlider.value = this.m_CurrentLaunchForce;
        }
        // Otherwise, if the fire button is released and the shell hasn't been launched yet...
        else if (Input.GetButtonUp(this.m_FireButton) && !this.m_Fired)
        {
            // ... launch the shell.
            this.Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        // Set the fired flag so only Fire is only called once.
        this.m_Fired = true;

        // Create an instance of the shell and store a reference to it's rigidbody.
        Rigidbody shellInstance =
            Instantiate(this.m_Shell, this.m_FireTransform.position, this.m_FireTransform.rotation) as Rigidbody;

        // Set the shell's velocity to the launch force in the fire position's forward direction.
        shellInstance.velocity = this.m_CurrentLaunchForce * this.m_FireTransform.forward;

        // Change the clip to the firing clip and play it.
        this.m_ShootingAudio.clip = this.m_FireClip;
        this.m_ShootingAudio.Play();

        // Reset the launch force.  This is a precaution in case of missing button events.
        this.m_CurrentLaunchForce = this.m_MinLaunchForce;

    }
}