using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Microsoft.Kinect;
//using Windows.Kinect;
using System.Linq;

public class TankControl : MonoBehaviour
{
    private float avgPositionZ;
    private float elbowPositionX;
    private float elbowPositionY;
    private TankShooting tankShooting;
    private TankMovement tankMovement;
    private uint userId;
    //private BodyFrameReader _bodyFrameReader;

    public int m_UserId;
    // the joint we want to track
    public KinectWrapper.NuiSkeletonPositionIndex handRight = KinectWrapper.NuiSkeletonPositionIndex.HandRight;
    public KinectWrapper.NuiSkeletonPositionIndex head = KinectWrapper.NuiSkeletonPositionIndex.Head;

    public KinectWrapper.NuiSkeletonPositionIndex handLeft = KinectWrapper.NuiSkeletonPositionIndex.HandLeft;
    public KinectWrapper.NuiSkeletonPositionIndex elbowLeft = KinectWrapper.NuiSkeletonPositionIndex.ElbowLeft;

    // Start is called before the first frame update
    void Start()
    {
        this.tankShooting = gameObject.GetComponent<TankShooting>();
        this.tankMovement = gameObject.GetComponent<TankMovement>();
    }

    private void Update()
    {
        // get the joint position
        KinectManager manager = KinectManager.Instance;

        if (manager && manager.IsInitialized())
        {
            if (manager.IsUserDetected())
            {
                if (this.m_UserId == 1)
                {
                    userId = manager.GetPlayer1ID();
                }
                else
                {
                    userId = manager.GetPlayer2ID();
                }

                if (manager.IsJointTracked(this.userId, (int)handRight) && manager.IsJointTracked(this.userId, (int)head))
                {
                    this.avgPositionZ = manager.GetJointPosition(this.userId, (int)head).z;
                    if (manager.GetJointPosition(this.userId, (int)handRight).z < avgPositionZ - 0.6)
                    {
                        //print("FIRE");
                        this.tankShooting.Fire();
                    }
                    else if (manager.GetJointPosition(this.userId, (int)handRight).z < avgPositionZ - 0.3)
                    {
                        //print("FIRING");
                        this.tankShooting.Firing();
                    }
                    else
                    {
                        //print("RELOAD");
                        this.tankShooting.Reload();
                    }
                }
                if (manager.IsJointTracked(this.userId, (int)handLeft) && manager.IsJointTracked(this.userId, (int)elbowLeft))
                {
                    this.elbowPositionX = manager.GetJointPosition(this.userId, (int)elbowLeft).x;
                    this.elbowPositionY = manager.GetJointPosition(this.userId, (int)elbowLeft).y;
                    this.tankMovement.changeTurnValue(manager.GetJointPosition(this.userId, (int)handLeft).x - this.elbowPositionX);
                    this.tankMovement.changeMovementValue(manager.GetJointPosition(this.userId, (int)handLeft).y - this.elbowPositionY);
                }

            }
        }
    }

}
