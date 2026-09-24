using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [System.Serializable]
    public class ColorKeyword
    {
        public string keyword;
        public Color color;
    }

    //Reference to the doors
    [SerializeField]
    public Transform[] doors;
    //The color used for each keyword
    [SerializeField]
    public ColorKeyword[] colorKeywords;
}
