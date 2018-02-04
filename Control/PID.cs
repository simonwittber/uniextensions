using UnityEngine;
using System.Collections;

namespace DifferentMethods.Extensions.Control
{

    /// <summary>
    /// A classic PID controller.
    /// </summary>
    [System.Serializable]
    public class PID
    {



        public float Kp = 0.5f;
        public float Ki = 0f;
        public float Kd = 0f;
        public float target;

        [System.NonSerialized]
        public float actual;
        [System.NonSerialized]
        public float signal;

        public bool useAngles = false;

        public PID()
        {
        }

        public PID(bool useAngles)
        {
            this.useAngles = useAngles;
        }


        public float Update(float actual)
        {
            this.actual = actual;
            return Update();
        }

        public float Update(float actual, float target)
        {
            this.actual = actual;
            this.target = target;
            return Update();
        }

        public float Update()
        {
            var error = useAngles ? Mathf.DeltaAngle(actual, target) : target - actual;
            var delta = error - error_last;
            error_sum += error;
            signal = Kp * error + Ki * error_sum + Kd * delta;
            error_last = error;
            return signal;
        }

        float error_last, error_sum;
    }
}
