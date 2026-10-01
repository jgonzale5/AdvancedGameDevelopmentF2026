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
    //A reference to the collider that we're using for checking obstructions during dungeon generation
    [SerializeField]
    public Collider colliderCheck;
}
