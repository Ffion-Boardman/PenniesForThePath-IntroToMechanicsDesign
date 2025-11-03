using Unity.Properties;
using UnityEngine;

public class TransformWrapper : MonoBehaviour
{
    [CreateProperty]
    public float XPos { get { return transform.position.x; } }
    [CreateProperty]
    public float YPos { get { return transform.position.y; } }
}
