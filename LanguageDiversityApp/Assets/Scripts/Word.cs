using UnityEngine;

[CreateAssetMenu(fileName = "Word", menuName = "ScriptableObjects/WordImageData", order = 1)]
public class Word : ScriptableObject
{
    public string keyword;
    public Sprite sprite;
}
