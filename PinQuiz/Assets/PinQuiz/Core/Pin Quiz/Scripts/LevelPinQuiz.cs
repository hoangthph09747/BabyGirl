using PinQuiz;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsPinQuiz", menuName = "ScriptableObjects/LevelsPinQuiz")]
public class LevelPinQuiz : ScriptableObject
{
    public PinQuizLevel[] levelsPrefab;
    public Sprite[] bgSprites;
}
