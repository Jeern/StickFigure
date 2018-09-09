namespace Beebapps.Game.Utils
{
    /// <summary>
    /// This code is here because PhotoChallenge could not compile with "Free" Conditional Constant
    /// For some reason the Mogade code did not accept the new constant. Therefore the code has been moved down to GameDev
    /// even though it does not belong here
    /// </summary>
    public static class Edition
    {
        public static bool IsFree
        {
            get 
            { 
#if FREE
                return true; 
#else
                return false;
#endif
            }
        }
    }
}
