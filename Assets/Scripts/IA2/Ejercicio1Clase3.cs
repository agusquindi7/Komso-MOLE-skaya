using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ejercicio1Clase3 : MonoBehaviour
{
    List<string> names = new List<string>();

    IEnumerable<string> GetCharacterNames(IEnumerable<CharacterModel> characters)
    {
        var list = characters.Select(x => x.name);

        return list;
    }

    IEnumerable<int> GetCharacterHps(IEnumerable<CharacterModel> characters)
    {
        var list = characters.Select(x => x.hp);

        return list;
    }

    IEnumerable<CharacterModel.Health> GetCharactersCondition(IEnumerable<CharacterModel> characters)
    {
        var listCond = characters.Select(x =>
        {
            float porcentaje = x.hp / x.maxHP;
            if (porcentaje >= 0.9f) return CharacterModel.Health.OK;
            else if (porcentaje >= 0.1f) return CharacterModel.Health.Damaged;
            else return CharacterModel.Health.NearDeath;
        });

        return listCond;
    }

    IEnumerable<CharacterModel.Health> GetNearDeathChars(IEnumerable<CharacterModel> characters)
    {
        return characters.Where(x => x.hp / x.maxHP < .1f)
                         .Select(x => CharacterModel.Health.NearDeath);
    }
    //EXTRA: EL ORDEN NO IMPORTA PORQUE ESTA SOLAMENTE DEVOLVIENDO UNA LISTA DE LOS CHARACTERS QUE ESTAN NEARDEATH

    IEnumerable<CharacterModel.Faction> GetCharsFaction(IEnumerable<CharacterModel> characters)
    {
        var listCond = characters.Select(x =>
        {
            if (x.color == Color.red) return CharacterModel.Faction.Enemy;
            else if (x.color == Color.blue) return CharacterModel.Faction.Ally;
            else return CharacterModel.Faction.Neutral;
        });
        return listCond;
    }

    IEnumerable<CharacterModel.Health> GetRedFactionNotOKChars(IEnumerable<CharacterModel> characters)
    {                           //ACA ME DEVUELVE LOS QUE NO ESTAN OK Y DE FACCION ROJA
        var list = characters.Where(x => x.color == Color.red && (x.hp / x.maxHP < .9f))
            .Select(x =>
        {
            float porcentaje = x.hp / x.maxHP;
            if (porcentaje >= 0.1f) return CharacterModel.Health.Damaged;
            else return CharacterModel.Health.NearDeath;
        }
        );
        return list;
    }

    IEnumerable<CharacterModel.Decision> AttackEnemies(IEnumerable<CharacterModel> characters)
    {
        return default;
    }
}
