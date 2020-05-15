namespace RPG.Control
{
    interface IRayCastables
    {
        CursorType GetCursorType();
        bool IsRayCastable(PlayerController callingController);
    }
}