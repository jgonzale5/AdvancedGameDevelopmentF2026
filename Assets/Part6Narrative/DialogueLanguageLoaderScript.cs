using System.Collections.Generic;
using UnityEngine;
//We add this library so we can access settings functions for localization
using UnityEngine.Localization.Settings;
public class DialogueLanguageLoaderScript : MonoBehaviour
{
    //A reference to this same component
    public static DialogueLanguageLoaderScript Instance;

    //The text asset where we'll be loading the language
    [SerializeField]
    private TextAsset languageFile;
    //Where we'll load the dialogue into
    private Dictionary<string, List<string>> loadedDialogue;
    //A dictionary that will tell us the index of a language
    private Dictionary<string, int> languageIndex;
    //
    private int languageSelected = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set the instance
        Instance = this;

        LoadDialogue();
    }

    //This function will receive an ID and return the string of the selected language
    //that corresponds to it
    public string GetLine(string ID)
    {
        //If we can get the value with the specific ID
        if (loadedDialogue.TryGetValue(ID, out List<string> list))
        {
            //If our language selection is not valid, we return an error.
            if (list.Count <= languageSelected)
            {
                return "ERROR::LANGUAGE NOT FOUND";
            }

            //If everything goes well, we return the dialogue text for the ID and language selected
            return list[languageSelected];
        }
        //If we couldn't find the ID we return an error
        return "ERROR::DIALOGUE WITH ID NOT FOUND";
    }

    //We use this function to tell the dialogue loader to change its language
    public void SelectLanguage(string languageID)
    {
        Debug.Log(languageID);
        //Find the locale with the specified ID and set it as the selected one
        LocalizationSettings.SelectedLocale = 
            LocalizationSettings.AvailableLocales.GetLocale(languageID);

        //If we can find the index in the dictionary
        if (languageIndex.TryGetValue(languageID, out int value))
        {
            //We set the language selected to it
            languageSelected = value;
            return;
        }

        //Otherwise, we display an error message;
        Debug.LogError("Couldn't change language. Language ID does not exist.");
    }

    //
    private void LoadDialogue()
    {
        //We split the file into lines
        string[] lines = languageFile.text.Split("\n");
        //Starting with the first line, we split it into elements using the commas
        string[] elements = lines[0].Split(",");
        //We initialize the index dictionary
        languageIndex = new Dictionary<string, int>();
        //Initialize loaded dialogue dictionary
        loadedDialogue = new Dictionary<string, List<string>>();

        //Assuming the first line is the headers, and the first two columns are not dialogue,
        //we build a dictionary to always know what index corresponds to each language
        for (int i = 2; i < elements.Length; i++)
        {
            languageIndex.Add(elements[i].Trim(), i - 2);
            Debug.Log("\"" + elements[i].Trim() + "\" " + (i - 2));
        }

        //Starting with the second line (after the ID) and going through each row:
        for (int y = 1; y < lines.Length -1; y++)
        {
            //Split the line into its elements using the commas
            elements = lines[y].Split(",");
            //Get the ID from the first column of the line
            string ID = elements[0];
            //Get the speaker from the second column, though it's not being used.
            //In the future it would've been better to make a custom class for dialogue
            //So we can store information like this
            string speaker = elements[1];
            //We initialize the list that's gonna hold the text for each language
            List<string> dialogueText = new List<string>();

            //We go through the third column onwards adding them to the list
            for (int x = 2; x < elements.Length; x++)
            {
                dialogueText.Add(elements[x]);
            }

            //We add the list and its corresponding ID to the loaded dialogue dictionary
            loadedDialogue.Add(ID, dialogueText);
        }
    }
}
