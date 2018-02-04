using UnityEngine;
using System.Collections;

namespace DifferentMethods.Extensions
{
    /// <summary>
    /// A Singleton manager.
    /// </summary>
    public class Singleton<T> where T : Component
    {
        static T instance = null;

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var g = GameObject.Find("/" + typeof(T).ToString());
                    if (g == null)
                    {
                        g = new GameObject(typeof(T).ToString(), typeof(T));
                    }
                    instance = g.GetComponent<T>();
                }
                return instance;
            }
        }
    }
}