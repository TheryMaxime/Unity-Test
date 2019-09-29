using System.Collections;
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
    public float m_MaxChargeTime = 20f;


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

    public void Fire()
    {
        if (!this.m_Fired)
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
            this.m_AimSlider.value = this.m_CurrentLaunchForce;
        }
    }

    public void Firing()
    {
        // If the max force has been exceeded and the shell hasn't yet been launched...
        if (this.m_CurrentLaunchForce >= this.m_MaxLaunchForce && !this.m_Fired)
        {
            // ... use the max force and launch the shell.
            this.m_CurrentLaunchForce = this.m_MaxLaunchForce;
            this.Fire();
        }
        else if (!this.m_Fired)
        {
            this.m_CurrentLaunchForce += this.m_ChargeSpeed * Time.deltaTime;

            this.m_AimSlider.value = this.m_CurrentLaunchForce;
        }
    }

    public void Reload()
    {
        this.m_Fired = false;
        this.m_CurrentLaunchForce = this.m_MinLaunchForce;
        this.m_AimSlider.value = this.m_CurrentLaunchForce;

        // Change the clip to the charging clip and start it playing.
        this.m_ShootingAudio.clip = this.m_ChargingClip;
        this.m_ShootingAudio.Play();
    }

}