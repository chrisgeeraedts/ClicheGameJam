namespace Assets.Scripts.OneManArmy
{
    public enum PlayerState
    {
        Idle,
        Moving,
        Attacking,
        TakingDamage,
        Dead
    }

    public enum MinigameState
    {
        Initialize,
        StoryMoment,
        Combat,
        PostCombat,
        Complete,
        ReturnToMap
    }

    public enum Direction
    { 
        Up,
        Down,
        Left,
        Right
    }
}
