using UnityEngine;

namespace NotMonk_Client.Classes;

public class CoroutineManager : MonoBehaviour
{
    public static CoroutineManager Instance;

    private void Awake() => Instance = this;
}