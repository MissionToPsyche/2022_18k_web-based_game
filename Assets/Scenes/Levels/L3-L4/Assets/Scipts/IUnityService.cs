using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scipts
{
    public interface IUnityService
    {
        float GetDeltaTime();
        float GetAxisRaw(string name);
    }

    class UnityService : IUnityService
    {
        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }
        public float GetAxisRaw(string name)
        {
            return Input.GetAxisRaw(name);
        }
    }
}
