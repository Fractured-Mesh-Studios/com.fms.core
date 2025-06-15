using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace CoreEngine.DI
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = false)]
    public class InjectAttribute : Attribute
    {
        // Este atributo es solo un marcador, no necesita propiedades adicionales por ahora.
    }
}
