using UnityEngine;
using System.Collections;
using System;

public class GestureListener_0 : MonoBehaviour, KinectGestures.GestureListenerInterface
{
    ///

    private bool pushing;
    private bool push;

    private int userIndex;

    public int GetUserIndex()
    {
        return this.userIndex;
    }

    public bool IsPush()
    {
        if (this.pushing)
        {
            this.push = false;
            this.pushing = false;
            return true;
        }
        return false;
    }

    public bool IsPushing()
    {
        if (this.pushing)
        {
            return true;
        }

        return false;
    }
    
    public void UserDetected(uint userId, int userIndex)
    {
        // detect these user specific gestures
        KinectManager manager = KinectManager.Instance;

        print(userIndex);
        manager.DetectGesture(userId, KinectGestures.Gestures.Push);


    }

    public void UserLost(uint userId, int userIndex)
    {
    }

    public void GestureInProgress(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                  float progress, KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        this.userIndex = userIndex;
        
        string sGestureText = gesture + "in progress :" + progress;
        //print(sGestureText);
        if (gesture == KinectGestures.Gestures.Push)
        {
            this.pushing = true;
            this.push = false;
        }
        
    }

    public bool GestureCompleted(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectWrapper.NuiSkeletonPositionIndex joint, Vector3 screenPos)
    {
        this.userIndex = userIndex;
        string sGestureText = gesture + " detected";
        //print(sGestureText);
        if (gesture == KinectGestures.Gestures.Push)
        {
            this.pushing = false;
            this.push = true;
        }
            

        return true;
    }

    public bool GestureCancelled(uint userId, int userIndex, KinectGestures.Gestures gesture,
                                  KinectWrapper.NuiSkeletonPositionIndex joint)
    {
        // don't do anything here, just reset the gesture state
        return true;
    }

}
