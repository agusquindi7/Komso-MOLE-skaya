using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using Unity;

public class CharacterModel
{
	public string name;
	public int maxHP;
	public int hp;
	public Color color;
	public Vector2 position;
	public Vector2 facing;

	public CharacterModel(string name, int maxHP, Color color, Vector2 position, Vector2 facing)
	{
		this.name = name;
		this.maxHP = maxHP;
		hp = maxHP;
		this.color = color;
		this.position = position;
		this.facing = facing;
	}

	public enum Health 
	{ 
		Damaged, 
		OK, 
		NearDeath 
	};
	public enum Faction { Ally, Neutral, Enemy };
	public enum Decision { Ignore, Follow, Attack }
}

//Escriba utilizando LINQ funciones que reciban un IEnumerable<CharacterModel> y retornen: 

//1. Una lista de nombres

//2. Una lista de HP’s

//3. Una lista con las condiciones de cada personaje, usando el enum Health
//OK: de 90 % a 100 % hp
//Damaged: de 10 % a 90 % hp
//NearDeath: debajo de 10 % de hp

//4. Una lista de condiciones de HP (enum Health) de personajes, manteniendo solo los que tengan hp debajo del 10%
//Extra: En este punto, el orden cambia en algo?

//5. Una lista con las facciones, usando el enum Faction : Rojos = enemigos, azules = amigos, el resto son neutrales.

//6. Una lista que contenga la condición de HP (enum Health) de los personajes, excluyendo los que estén “OK”, solamente evaluando personajes de la facción roja.

//7. Lista de decisiones: Atacar a los enemigos, ignorar al resto.

//Lista de decisiones: Atacar a cualquiera que esté por debajo de 10% de hp, ignorar al resto.
//Lista de decisiones: Atacar a cualquiera que esté por arriba de 10% de hp, sin evaluar a aquellos que sean enemigos, ignorar al resto.
//Lista de decisiones: atacar a enemigos y neutrales que estén por debajo de 10% HP, ignorar al resto.
