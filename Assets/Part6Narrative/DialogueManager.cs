using UnityEngine;
//We import the inkle library so we can interact with it
using Ink.Runtime;
using Ink.UnityIntegration;

public class DialogueManager : MonoBehaviour
{
    //This is way to refer to this script from anywhere else
    public static DialogueManager instance;

    //
    [SerializeField]
    private DialogueUIManager uiManager;

    //A scene is a file with a "conversation" or narrative. Basically an ink file.
    [SerializeField]
    private InkFile scene;

    //This just holds a reference to the active scene, because we can switch it.
    private Story activeScene;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //An instance so we can easily refer to this object later
        instance = this;

        //Take the text from the scene file and load it into a story object
        activeScene = new Story(scene.storyJson);

        //Kickstart the story
        Next();
    }

    //This function progresses the story and displays the new text
    public void Next()
    {
        //If the scene can continue
        if (activeScene.canContinue)
        {
            //Continue the story and cache what it says
            string line = activeScene.Continue();

            //If there are any tags in the current line
            if (activeScene.currentTags.Count > 0)
            {
                //Get the line from the language loader
                line = DialogueLanguageLoaderScript.Instance.
                    GetLine(activeScene.currentTags[0]);
            }

            //If there are choices present now
            if (activeScene.currentChoices.Count > 0)
            {
                //Clear the ones in the UI
                uiManager.ClearChoices();

                //Spawn new ones
                for (int i = 0; i < activeScene.currentChoices.Count; i++)
                {
                    string choiceText = activeScene.currentChoices[i].text;

                    //If there are any tags in the current line
                    if (activeScene.currentTags.Count > 0)
                    {
                        //Get the text from the language loader
                        choiceText = DialogueLanguageLoaderScript.Instance.
                            GetLine(activeScene.currentChoices[i].tags[0]);
                    }

                    //Add the choice with the index that corresponds to it
                    uiManager.AddChoice(choiceText, i);
                }
            }

            //Display the text
            uiManager.DisplayText(line);
        }
    }

    //This function tells the active scene the decision made and progresses the story
    public void MakeChoice(int choice)
    {
        //Tell the active scene what choice was made
        activeScene.ChooseChoiceIndex(choice);
        //Move forward
        Next();
        //Clear the choices
        uiManager.ClearChoices();
    }
}
