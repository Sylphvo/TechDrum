namespace TechDrum.Core.Constants
{
    public enum FundistType
    {
        HashCreateUser,
        HashUpdateUser,
        HashSetBalance,
        HashGetBalance,
        HashGetWithdrawBalance,
        /// <summary>
        /// Load iframe game by auth
        /// </summary>
        HashDirectAuth,
        /// <summary>
        /// Load iframe game by auth html
        /// </summary>
        HashAuthHtml,
        /// <summary>
        /// Reject login multiple
        /// </summary>
        HashKillAuth,
        HashUniverBetsAuth,
        HashStatsBetAuth,
        HashDetailStateAuth,
        /// <summary>
        /// Load game detail users
        /// </summary>
        HashGameDetail,
        HashEnabledUser,
        /// <summary>
        /// Load full type game
        /// </summary>
        HashFullListGame
    }
}
