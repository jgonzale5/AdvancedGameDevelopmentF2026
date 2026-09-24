using System.Collections.Generic;
using UnityEngine;

public class DungeonManagerScript : MonoBehaviour
{
    //A class to keep track of the exits currently open in the dungeon
    private class RoomExits
    {
        public Transform edge;
        public string keyword;

        //Constructor for the class, we send it an edge and a keyword as its 
        // initial value
        public RoomExits(Transform edge, string keyword)
        {
            this.keyword = keyword;
            this.edge = edge;
        }
    }

    [SerializeField]
    private DungeonSettingsSO settings;

    //The deck of rooms to spawn
    private List<DungeonRoomSO> deck = new();
    //Rooms currently spawned
    private List<RoomScript> currentRooms = new();
    //This is a dynamic list to keep track of which exits are open in the dungeon
    private List<RoomExits> availableExits = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateDungeon();
    }

    public void GenerateDungeon()
    {
        int roomCount = Random.Range(settings.minRoomCount,
            settings.maxRoomCount);
        DungeonRoomSO newRoom;

        //Stock the deck with the cards we can have
        RestockDeck();
        //Shuffle
        ShuffleDeck();
        //Get a new room
        Draw(out newRoom);

        //Spawn the room at 0,0,0 with no rotation
        //Store a reference in "currentRoom"
        //We'll use it to keep track of new rooms
        RoomScript currentRoom = Instantiate(newRoom.prefab, 
            Vector3.zero, Quaternion.identity);

        //For each exit in the room we just spawned
        for (int n = 0; n < currentRoom.doors.Length; n++) 
        {
            //Add it to the list of available exits
            availableExits.Add(new RoomExits(
                currentRoom.doors[n], currentRoom.colorKeywords[n].keyword));
        }

        //Repeat for as many room as we want
        for (int i = 1; i < roomCount; i++)
        {
            //If we couldn't draw a room
            if (!Draw(out newRoom))
            {
                //Shuffle
                RestockDeck();
                //Draw again
                Draw(out newRoom);
            }

            SpawnRoom(newRoom, currentRoom);
            
        }
    }
    void SpawnRoom(DungeonRoomSO newRoom, RoomScript currentRoom)
    {
        RoomExits currentRoomExit;

        int exitInd = 0;
        foreach (var exit in newRoom.roomDoors)
        {
            foreach (var possibleConnections in exit.canConnectToKeywords)
            {
                if ((currentRoomExit = availableExits.Find
                    (x => x.keyword == possibleConnections)) != null)
                {
                    //Spawn the next room
                    // more details to come
                    currentRoom = Instantiate(newRoom.prefab,
                        Vector3.zero, Quaternion.identity);

                    //Get a reference to the selected exit
                    Transform selectedExit = currentRoom.doors[exitInd].transform;
                    //Parent room to exit
                    selectedExit.SetParent(null);
                    currentRoom.transform.SetParent(selectedExit);
                    //Align door with available exit and make them point to each other
                    selectedExit.position = currentRoomExit.edge.position;
                    selectedExit.forward = -currentRoomExit.edge.forward;

                    //For each exit in the room we just spawned
                    for (int n = 0; n < currentRoom.doors.Length; n++)
                    {
                        //Add it to the list of available exits
                        availableExits.Add(new RoomExits(
                            currentRoom.doors[n], currentRoom.colorKeywords[n].keyword));
                    }

                    //Shuffle the list of available exits
                    for (int j = 0; j < availableExits.Count; j++)
                    {
                        int rand = Random.Range(0, availableExits.Count);
                        RoomExits temp = availableExits[j];
                        availableExits[j] = availableExits[rand];
                        availableExits[rand] = temp;
                    }
                }

                availableExits.Remove(currentRoomExit);

                return;
            }
            exitInd++;
        }
    }

    public void ShuffleDeck()
    {
        DungeonRoomSO temp;

        for (int i = 0; i < deck.Count; i++)
        {
            //choose a random position on the deck
            int rand = Random.Range(0, deck.Count);
            //Swap this element with that one
            temp = deck[rand];
            deck[rand] = deck[i];
            deck[i] = temp;
        }
    }

    public void RestockDeck()
    {
        //Clear the deck
        deck.Clear();

        //For each item in the dungeon settings
        foreach (DungeonSettingsSO.RoomInventoryItem item
            in settings.possibleRooms)
        {
            //Add that many "cards" of that type to the deck
            for (int i = 0; i < item.copies; i++)
            {
                deck.Add(item.room);
            }
        }
    }

    /// <summary>
    /// A function that removes a card from the deck and gives it back.
    /// </summary>
    /// <param name="room">Where the drawn "card" will go.</param>
    /// <returns>Whether the draw was succesful.</returns>
    public bool Draw(out DungeonRoomSO room)
    {
        //Set a default value for room
        room = null;

        //If the deck isn't empty
        if (deck.Count > 0)
        {
            //Put the first card in the room variable
            room = deck[0];
            //Remove it from the deck
            deck.RemoveAt(0);
            //Return true
            return true;
        }

        //Return false if the deck is empty
        return false;
    }
}
