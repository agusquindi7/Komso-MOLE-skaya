using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class RewindController : NetworkBehaviour
{
    public TrailRenderer trail;

    [Header("Ajustes")]
    public bool useInstantTeleport = true;
    public KeyCode abilityTP = KeyCode.R;
    public float rewindDuration = 5f;
    public float playbackSpeed = 1f;

    private NetworkCharacterController _ncc;
    private float _time;

    // Rewind
    private bool _rewindActive = false;
    private List<Vector3> _rewindPositions = new();
    private int _rewindIndex;

    private PositionOriginator originator = new();
    private Caretaker caretaker = new();

    private void Awake()
    {
        _ncc = GetComponent<NetworkCharacterController>();
    }

    public override void FixedUpdateNetwork()
    {
        _time = Runner.Tick * Runner.DeltaTime;

        // INPUT: solo el dueño detecta
        if (HasInputAuthority && Input.GetKeyDown(abilityTP))
        {
            if (useInstantTeleport)
                RPC_RequestInstantTeleport();
            else
                RPC_RequestRewind();
        }

        // REWIND activo (solo Host lo ejecuta)
        if (HasStateAuthority && _rewindActive)
        {
            if (_rewindIndex >= 0)
            {
                Vector3 pos = _rewindPositions[_rewindIndex];
                if (!float.IsNaN(pos.x)) _ncc.Teleport(pos);
                _rewindIndex--;
            }
            else
            {
                _rewindActive = false;
            }
            return;
        }

        // Guardar estado (solo Host)
        if (HasStateAuthority)
        {
            originator.SetState(transform.position);
            caretaker.Add(originator.CreateMemento(), _time);
            caretaker.TrimOlderThan(_time - rewindDuration);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestInstantTeleport()
    {
        Vector3 rewindPos = GetStateAt(_time - rewindDuration);
        if (!float.IsNaN(rewindPos.x)) _ncc.Teleport(rewindPos);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_RequestRewind()
    {
        StartRewindPlayback();
    }

    private void StartRewindPlayback()
    {
        _rewindActive = true;
        float cutoff = _time - rewindDuration;

        var clip = caretaker.History.FindAll(e => e.time >= cutoff && e.time <= _time);
        clip.Sort((a, b) => a.time.CompareTo(b.time));

        _rewindPositions.Clear();
        foreach (var entry in clip)
            _rewindPositions.Add(entry.m.SavedState);

        _rewindIndex = _rewindPositions.Count - 1;
    }

    private Vector3 GetStateAt(float targetTime)
    {
        var history = caretaker.History;

        if (history.Count < 2)
            return transform.position;

        (Memento m, float time) prev = history[0];

        foreach (var entry in history)
        {
            if (entry.time >= targetTime)
            {
                float denominator = entry.time - prev.time;
                if (Mathf.Approximately(denominator, 0f))
                    return prev.m.SavedState;

                float t = (targetTime - prev.time) / denominator;
                Vector3 result = Vector3.Lerp(prev.m.SavedState, entry.m.SavedState, t);

                if (float.IsNaN(result.x)) return prev.m.SavedState;
                return result;
            }

            prev = entry;
        }

        return history[^1].m.SavedState;
    }

    // MEMENTO PATTERN
    private class PositionOriginator
    {
        private Vector3 _state;
        public void SetState(Vector3 pos) => _state = pos;
        public Memento CreateMemento() => new Memento(_state);
    }

    private class Memento
    {
        public Vector3 SavedState { get; }
        public Memento(Vector3 state) { SavedState = state; }
    }

    private class Caretaker
    {
        public List<(Memento m, float time)> History { get; } = new();
        public void Add(Memento m, float t) => History.Add((m, t));
        public void TrimOlderThan(float cutoff) => History.RemoveAll(entry => entry.time < cutoff);
    }
}
