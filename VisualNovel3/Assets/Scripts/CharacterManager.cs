using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Responsible for adding and maintaining character in the scene
/// </summary>
public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;

    /// <summary>
    /// All characters must be attached to character panel.
    /// </summary>
    public RectTransform characterPanel;

    /// <summary>
    /// A list of characters currently in our scene.
    /// </summary>
    public List<Character> characters = new List<Character>();

    /// <summary>
    /// Easy lookup for our characters.
    /// </summary>
    public Dictionary<string, int> characterDictionary = new Dictionary<string, int>();

    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("!!! Tring to create multiple instance of CharacterManager !!!");
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Try to get a character by the name provided from the character list.
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="createCharacterIdDoesNotExist"></param>
    /// <param name="enableCreatedCharacterOnStart"></param>
    /// <returns></returns>
    public Character GetCharacter(string characterName, bool createCharacterIdDoesNotExist = true, bool enableCreatedCharacterOnStart = true)
    {
        //search our dictionary to find the character quickly if it is already in our scene. 
        int index = -1;
        if (characterDictionary.TryGetValue(characterName, out index))
        {
            return characters[index];
        }
        else
        {
            return CreateCharacter(characterName ,enableCreatedCharacterOnStart);
        }
    }

    /// <summary>
    /// Create the character.
    /// </summary>
    /// <param name="characterName"></param>
    /// <param name="enabledOnStart"></param>
    /// <returns></returns>
    public Character CreateCharacter(string characterName, bool enabledOnStart = true)
    {
        Character newCharacter = new Character(characterName, enabledOnStart);

        characterDictionary.Add(characterName, characters.Count);
        characters.Add(newCharacter);

        return newCharacter;
    }

    public class CHARACTERPOSITIONS
    {
        public Vector2 bottomLeft = new Vector2(0, 0);
        public Vector2 topRight = new Vector2(1f, 1f);
        public Vector2 center = new Vector2(0.5f, 0.5f);
        public Vector2 bottomRight = new Vector2(1f, 0);
        public Vector2 topLeft = new Vector2(0, 1f);
    }
    public static CHARACTERPOSITIONS characterPositions = new CHARACTERPOSITIONS();

    public class CHARACTEREXPRESSIONS
    {
        public int normal = 0;
        public int shy = 1;
        public int normalAngle = 2;
        public int cojoinedFingers = 3;
        public int a = 5;
        public int b = 6;
        public int c = 8;
    }

    public static CHARACTEREXPRESSIONS characterExpressions = new CHARACTEREXPRESSIONS();
}
