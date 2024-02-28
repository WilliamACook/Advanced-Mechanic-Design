using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class TypeParser : MonoBehaviour
{
    int Health = 0;
    int Speed = 0;
    int Damage = 0;

    public void ParsedBlock(ParsedBlock parsedBlock)
    {       
        string input = parsedBlock.content;
        string pattern = @"health=>(\d+)!\?speed=>(\d+)!\?damage=>(\d+)!";

        Regex regex = new Regex(pattern);
        MatchCollection matches = regex.Matches(input);

        foreach(Match match in matches)
        {
            if (match.Success)
            {
                string health = match.Groups[1].Value;
                string speed = match.Groups[2].Value;
                string damage = match.Groups[3].Value;

                if (int.TryParse(health, out Health) && int.TryParse(speed, out Speed) && int.TryParse(damage, out Damage))
                {
                    print("Health: " + Health);
                    print("Speed: " + Speed);
                    print("Damage: " + Damage);
                }

                //print(Health);
            }

        }




    }
}
