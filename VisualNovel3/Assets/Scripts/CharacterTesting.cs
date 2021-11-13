using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTesting : MonoBehaviour
{
    public Character Raelin;
    public Vector2 moveTarget;
    public float moveSpeed;
    public bool smooth;

    public int bodyIndex, expressionIndex = 0;
    public float speed = 5;
    public bool smoothTransition = false;

    // Start is called before the first frame update
    void Start()
    {
        Raelin = CharacterManager.instance.GetCharacter("Raelin", enableCreatedCharacterOnStart : true);
    }

    public string[] speech;
    int i = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(i < speech.Length)
            {
                Raelin.Say(speech[i]);
            }
            else
            {
                DialogueSystem.instance.Close();
            }
            i++;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            Raelin.MoveTo(moveTarget, moveSpeed,smooth);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Raelin.StopMoving();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.T))
            {
                Debug.Log(CharacterManager.characterExpressions.cojoinedFingers);
                Raelin.TransitionBody(Raelin.GetSprite(CharacterManager.characterExpressions.cojoinedFingers), speed, smoothTransition);
            }
            else
            {
                Raelin.SetBody(bodyIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.T))
            {
                Raelin.TransitionExpression(Raelin.GetSprite(CharacterManager.characterExpressions.b), speed, smoothTransition);
            }
            else
            {
                Raelin.SetExpression(expressionIndex);
            }
        }
    }
}
