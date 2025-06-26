using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TestIA2 : MonoBehaviour
{
    [SerializeField] List<int> numbers = new List<int>();
    [SerializeField] List<string> words = new List<string>();
    [SerializeField] List<Enemy> enemy = new List<Enemy>();

    private void Start()
    {
        numbers = EvenNumbers(numbers).ToList();
        words = AllWordsFirstWithA(words).ToList();
    }

    IEnumerable<int> EvenNumbers(List<int> numbers)
    {
        foreach (int number in numbers)
        {
            if (number % 2 == 0)
            {
                Debug.Log(number);
                yield return number;
            }
        }
    }
    IEnumerable<string> AllWordsFirstWithA(List<string> words)
    {
        foreach (var word in words)
        {
            char firstLetter = char.ToUpper(word[0]);
            if (firstLetter == 'A')
            {
                Debug.Log(word);
                yield return word;
            }
            else
                yield return null;
        }
    }

    IEnumerable<Enemy> EnemyRandomizer(List<Enemy> enemies)
    {
        var temp = enemies;
        foreach(Enemy enemy in enemies)
        {
            var random = Random.value;
            yield return default;
        }
    }
}

public class Enemy
{
    int life;
    public enum EnemyType
    {
        Orco,
        Goblin,
        Troll
    }
}