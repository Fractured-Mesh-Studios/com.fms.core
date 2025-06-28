using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CoreEngine
{
    public interface IBaseService
    { }

    public interface IServiceRegister : IBaseService
    {
        public void Register();
        public void Unregister();
    }
}
