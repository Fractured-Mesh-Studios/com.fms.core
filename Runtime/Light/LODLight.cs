using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameEngine
{
    [RequireComponent(typeof(Light))]
    public class LODLight : MonoBehaviour
    {
        [Range(0,1)] public float updateDelay = 0.1f;

        [SerializeField] private LODAdjustment[] m_LODLevel;
        private Coroutine m_Coroutine;
        private Light m_Light;
        private Camera m_Camera;

        private void Awake()
        {
            m_Camera = Camera.main;
            m_Light = GetComponent<Light>();
        }

        private void OnEnable()
        {
            m_Coroutine = StartCoroutine(AdjustLOD());
        }

        private void OnDisable()
        {
            StopCoroutine(m_Coroutine);
        }

        private IEnumerator AdjustLOD()
        {
            float delay = updateDelay + updateDelay == 0 ? updateDelay : Random.value / 20f;
            WaitForSeconds wait = new WaitForSeconds(delay);
    
            while (true)
            {
                if(m_Camera == null)
                {
                    yield return null;
                    continue;
                }

                if (m_Light.enabled)
                {
                    float squareDistanceCamera = Vector3.SqrMagnitude(m_Camera.transform.position - transform.position);

                    for(int i = 0; i < m_LODLevel.Length; i++)
                    {
                        if (i == m_LODLevel.Length - 1 || SquareDistance(i, squareDistanceCamera))
                        {
                            m_Light.shadows = m_LODLevel[i].lightShadows;
                            m_Light.shadowResolution = ClampResolution(m_LODLevel[i].resolution);
                            m_Light.shadowStrength = m_LODLevel[i].strength;
                            i = m_LODLevel.Length;
                        }
                    }
                }

                yield return wait;
            }

        }

        #region PRIVATE
        private LightShadowResolution ClampResolution(ShadowResolution resolution)
        {
            if (QualitySettings.shadowResolution <= resolution)
                return (LightShadowResolution)QualitySettings.shadowResolution;
            else
                return (LightShadowResolution)resolution;
        }

        private bool SquareDistance(int index, float distance)
        {
            return (distance > m_LODLevel[index].minSquareDistance && distance <= m_LODLevel[index].maxSquareDistance);
        }
        #endregion
    }
}
