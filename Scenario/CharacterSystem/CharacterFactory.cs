using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CharacterFactory")]
public class CharacterFactory : ScriptableObject
{
    [SerializeField] private List<CharacterStruct> _characterList;

    public static CharacterFactory singleton;

    public void Init()
    {
        singleton = this;
    }

    public CharacterStruct GetCharacter(CharacterName name)
    {
        int characterIndex = 0;
        CharacterStruct character = new CharacterStruct();

        for (int i = 0; i < _characterList.Count; i++)
        {
            if (_characterList[i].nameEnum == name)
            {
                characterIndex = i;
                break;
            }
        }

        return _characterList[characterIndex];
    }
}
