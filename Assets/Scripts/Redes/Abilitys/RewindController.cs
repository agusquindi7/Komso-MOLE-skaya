using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindController : MonoBehaviour
{
    public TrailRenderer trail;

    [Header("Valores")]
    [Tooltip("True: Instant TP; False: Rewind")]
    public bool useInstantTeleport = true;
    public KeyCode abilityTP= KeyCode.R;
    [Tooltip("Duracion del rewind")]
    public float rewindDuration = 5f;
    [Tooltip("Velocidad para reproducir los mementos (1 = tiempo real)")]
    public float playbackSpeed = 1f;
    public float cdAbility = 5f;


    //Originator(el objeto que tiene estado) toma su propia posición(transform.position) y crea un “snapshot” (memento) cada vez que quiera registrar un estado
    //Originator: gestiona su propio estado y crea/restaura mementos
    private class PositionOriginator
    {
        private Vector3 _state; //creo un estado
        public void SetState(Vector3 pos) => _state = pos; //le seteo el vector3 de la posicion
        public Memento CreateMemento() => new Memento(_state); //creo un mememnto con ese estado
        public void RestoreMemento(Memento m) => _state = m.SavedState; //
        public Vector3 GetState() => _state;
    }

    // Memento: snapshot de posicion
    private class Memento
    {
        public Vector3 SavedState { get; }
        public Memento(Vector3 state) { SavedState = state; }
    }

    //Caretaker (quien guarda snapshots) mantiene una lista de pares (memento, timestamp) y la poda para descartar entradas más antiguas que 5 s
    // Caretaker: almacena mementos con timestamps
    private class Caretaker
    {
        public List<(Memento m, float time)> History { get; } = new List<(Memento, float)>(); //creo una lista HISTORIAL del MEMENTO (posicion) y tiempo, una tupla 
        public void Add(Memento m, float t) => History.Add((m, t)); //agrego a la lista del historial un elemento
        public void TrimOlderThan(float cutoff) //metodo para elminar historial si el tiempo transcurrido es mayor al limite establecido
        {
            History.RemoveAll(entry => entry.time < cutoff);
        }
    }

    // creo los scripts en este mismo y de paso tengo una referencia (?
    private PositionOriginator originator = new PositionOriginator();
    private Caretaker caretaker = new Caretaker();

    void Update()
    {
        // guardo el estado actual en cada frame. seteo y agrego
        originator.SetState(transform.position);
        caretaker.Add(originator.CreateMemento(), Time.time);
        caretaker.TrimOlderThan(Time.time - rewindDuration); //si el tiempo es superior al tiempo de rewind lo elimino de la lista

        if (Input.GetKeyDown(abilityTP))
        {
            if (useInstantTeleport) // hacer tp a donde estaba hace X segundos
            {
                float targetTime = Time.time - rewindDuration;
                Vector3 pos = GetStateAt(targetTime);
                transform.position = pos;
            }
            else //recorrido de mementos
            {
                
                StartCoroutine(PlaybackMementos());
            }
        }
    }  
    private Vector3 GetStateAt(float targetTime) //busco la pos hace X segundos
    {
        var history = caretaker.History; //guardo el historial de la lista en una var asi cada vez que entra al metodo la reuso
        if (history.Count == 0) return transform.position; //por las dudas ya que una vez aprete rapido y crasheo al no tener elementos guardados
                
        (Memento m, float time) prev = history[0]; //la primera posicion del historial
        foreach (var entry in history)
        {
            if (entry.time >= targetTime)
            {
                // Interpolar entre prev y entry
                float t = (targetTime - prev.time) / (entry.time - prev.time);
                Vector3 a = prev.m.SavedState;
                Vector3 b = entry.m.SavedState;
                //porque hace una interpolacion? para evitar saltos bruscos, buscar una pos por tiempo no es 100% exacto y depende de la pc de c/u. lerp te deja un resultado mas limpio que un punto exacto
                return Vector3.Lerp(a, b, t);
            }
            prev = entry;
        }
        // si no encuentra entry >= targetTime, devuelve el primer o ultimo elemento agregado
        return history[0].time >= targetTime ? history[0].m.SavedState : history[history.Count - 1].m.SavedState;
    }

    private IEnumerator PlaybackMementos()
    {
        float startTime = Time.time;
        float cutoff = startTime - rewindDuration;
        var history = caretaker.History;

        // filtra los mementos entre cutoff y ahora y los guardo en una lista
        List<(Memento m, float time)> clip = history.FindAll(e => e.time >= cutoff && e.time <= startTime);
        // ordeno por tiempo ascendente, luego recorro al reves en un for
        clip.Sort((a, b) => a.time.CompareTo(b.time));

        for (int i = clip.Count - 1; i >= 0; i--)
        {
            var entry = clip[i];
            originator.RestoreMemento(entry.m);
            transform.position = originator.GetState();

            // Esperar el delta de tiempo entre este y el anterior, ajustado por playbackSpeed
            if (i > 0)
            {
                float delta = (clip[i].time - clip[i - 1].time) / playbackSpeed;
                yield return new WaitForSeconds(delta);
            }
        }
    }
}
