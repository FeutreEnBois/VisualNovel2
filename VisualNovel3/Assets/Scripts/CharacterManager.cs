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
    public RectTransform characterPanel;
    public List<Character> characters = new List<Character>();
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

    public Character GetCharacter(string characterName, bool createCharacterIdDoesNotExist = true)
    {
        int index = -1;
        if (characterDictionary.TryGetValue(characterName, out index))
        {
            return characters[index];
        }
        else
        {
            return CreateCharacter(characterName);
        }
    }

    public Character CreateCharacter(string characterName)
    {
        Character newCharacter = new Character(characterName);
        characterDictionary.Add(characterName, characters.Count);
        characters.Add(newCharacter);
        return newCharacter;
    }

}
