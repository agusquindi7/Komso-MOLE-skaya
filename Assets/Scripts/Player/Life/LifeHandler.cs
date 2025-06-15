using Fusion;

public class LifeHandler : NetworkBehaviour
{
    private byte _currentLife;

    private const byte MAX_LIFE = 100;

    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            _currentLife = MAX_LIFE;
        }
    }

    public void TakeDamage(byte dmg)
    {
        if (dmg > _currentLife) dmg = _currentLife;

        _currentLife -= dmg;

        if (_currentLife != 0) return;
        
        DisconnectPlayer();
    }

    void DisconnectPlayer()
    {
        if (!Object.HasInputAuthority)
        {
            Runner.Disconnect(Object.InputAuthority);
        }
        
        Runner.Despawn(Object);
    }
}