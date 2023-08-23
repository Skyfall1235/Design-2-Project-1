using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysUpNav : MonoBehaviour
{
    [SerializeField] private GameObject m_groundPad;
    // Update is called once per frame
    void Update()
    {
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, m_groundPad.transform.up) * transform.rotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 5 * Time.deltaTime);
    }
}
