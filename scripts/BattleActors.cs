using Godot.Collections;

public sealed class BattleActors
{
    private Array<Player> _players;
    private Array<Enemy> _enemies;

    private BattleActors() { }
    private static BattleActors _instance;
    private Actor _currentTarget;

    public static BattleActors GetInstance()
    {
        if (_instance == null)
        {
            _instance = new BattleActors();
        }
        return _instance;
    }

    public void setEnemies(Array<Enemy> enemies)
    {
        _enemies = enemies;
    }

    public void setPlayers(Array<Player> players)
    {
        _players = players;
    }

    public Player getRandomPlayer()
    {
        return _players[0];
    }

    public Enemy getRandomEnemy()
    {
        return _enemies[0];
    }

    public void setPlayerTarget(Actor target)
    {
        _currentTarget = target;
    }

    public Actor getCurrentTarget()
    {
        return _currentTarget;
    }
}