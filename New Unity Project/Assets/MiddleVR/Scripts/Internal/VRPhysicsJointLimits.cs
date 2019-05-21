/* VRPhysicsJointLimits
 * MiddleVR
 * (c) MiddleVR
 */

using UnityEngine;
using System;

[Serializable]
public class VRPhysicsJointLimits
{
    public double Min
    {
        get
        {
            return m_Min;
        }

        set
        {
            m_Min = value;
        }
    }

    public double Max
    {
        get
        {
            return m_Max;
        }

        set
        {
            m_Max = value;
        }
    }

    [SerializeField]
    private double m_Min;

    [SerializeField]
    private double m_Max;

    public VRPhysicsJointLimits() : this(0.0)
    {
    }

    public VRPhysicsJointLimits(double iMinVal): this(iMinVal, 0.0)
    {
    }

    public VRPhysicsJointLimits(double iMinVal, double iMaxVal)
    {
        m_Min = iMinVal;
        m_Max = iMaxVal;
    }
}
