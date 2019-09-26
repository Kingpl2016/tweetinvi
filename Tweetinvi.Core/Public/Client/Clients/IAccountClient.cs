using System.Threading.Tasks;
using Tweetinvi.Iterators;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace Tweetinvi.Client
{
    /// <summary>
    /// AccountsClient contains all the operations that can be executed from an authenticated user's context.
    /// </summary>
    public interface IAccountClient
    {
        #region AuthenticatedUser

        /// <inheritdoc cref="GetAuthenticatedUser(IGetAuthenticatedUserParameters)" />
        Task<IAuthenticatedUser> GetAuthenticatedUser();

        /// <summary>
        /// Get the authenticated user based on the client's credentials
        /// <para>Read more : https://dev.twitter.com/rest/reference/get/account/verify_credentials</para>
        /// </summary>
        /// <returns>The client's authenticated user</returns>
        Task<IAuthenticatedUser> GetAuthenticatedUser(IGetAuthenticatedUserParameters parameters);

        #endregion

        #region Block / Unblock

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task<bool> BlockUser(long? userId);

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task<bool> BlockUser(string username);

        /// <inheritdoc cref="BlockUser(IBlockUserParameters)" />
        Task<bool> BlockUser(IUserIdentifier user);

        /// <summary>
        /// Block a user from the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-create</para>
        /// </summary>
        /// <returns>Whether the block operation was successful</returns>
        Task<bool> BlockUser(IBlockUserParameters parameters);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<bool> UnblockUser(long? userId);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<bool> UnblockUser(string username);

        /// <inheritdoc cref="UnblockUser(IUnblockUserParameters)" />
        Task<bool> UnBlockUser(IUserIdentifier user);

        /// <summary>
        /// Unblock a user from the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-blocks-destroy</para>
        /// </summary>
        /// <returns>Whether the unblock operation was successful</returns>
        Task<bool> UnblockUser(IUnblockUserParameters parameters);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<bool> ReportUserForSpam(long? userId);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<bool> ReportUserForSpam(string username);

        /// <inheritdoc cref="ReportUserForSpam(IReportUserForSpamParameters)" />
        Task<bool> ReportUserForSpam(IUserIdentifier user);

        /// <summary>
        /// Report a user for spam
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/post-users-report_spam</para>
        /// </summary>
        /// <returns>Whether the report was successful</returns>
        Task<bool> ReportUserForSpam(IReportUserForSpamParameters parameters);

        /// <inheritdoc cref="GetBlockedUserIds(IGetBlockedUserIdsParameters)" />
        ITwitterIterator<long> GetBlockedUserIds();

        /// <summary>
        /// Get the user ids blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids</para>
        /// </summary>
        /// <returns>An iterator to list the blocked users</returns>
        ITwitterIterator<long> GetBlockedUserIds(IGetBlockedUserIdsParameters parameters);

        /// <inheritdoc cref="GetBlockedUsers(IGetBlockedUsersParameters)" />
        ITwitterIterator<IUser> GetBlockedUsers();

        /// <summary>
        /// Get the users blocked by the client's account
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/mute-block-report-users/api-reference/get-blocks-ids</para>
        /// </summary>
        /// <returns>An iterator to list the blocked users</returns>
        ITwitterIterator<IUser> GetBlockedUsers(IGetBlockedUsersParameters parameters);

        #endregion

        #region Follow / Unfollow

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task<bool> FollowUser(long userId);

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task<bool> FollowUser(string username);

        /// <inheritdoc cref="FollowUser(IFollowUserParameters)" />
        Task<bool> FollowUser(IUserIdentifier user);

        /// <summary>
        /// Follow a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-create</para>
        /// </summary>
        /// <returns>Whether the follow operation was successful</returns>
        Task<bool> FollowUser(IFollowUserParameters parameters);

        /// <inheritdoc cref="UnFollowUser(IUnFollowUserParameters)" />
        Task<bool> UnFollowUser(long userId);

        /// <inheritdoc cref="UnFollowUser(IUnFollowUserParameters)" />
        Task<bool> UnFollowUser(string username);

        /// <inheritdoc cref="UnFollowUser(IUnFollowUserParameters)" />
        Task<bool> UnFollowUser(IUserIdentifier user);

        /// <summary>
        /// Stops following a user
        /// <para>Read more : https://dev.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/post-friendships-destroy</para>
        /// </summary>
        /// <returns>Whether the unfollow operation was successful</returns>
        Task<bool> UnFollowUser(IUnFollowUserParameters parameters);

        #endregion

        #region Follower Requests

        /// <inheritdoc cref="GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters)" />
        ITwitterIterator<long> GetUserIdsRequestingFriendship();

        /// <summary>
        /// Get the pending follower requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming</para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>An iterator to list the user ids who requested to follow the client's account</returns>
        ITwitterIterator<long> GetUserIdsRequestingFriendship(IGetUserIdsRequestingFriendshipParameters parameters);

        /// <inheritdoc cref="GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters)" />
        IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship();

        /// <summary>
        /// Get the pending follower requests for protected accounts.
        /// <para>Read more : https://developer.twitter.com/en/docs/accounts-and-users/follow-search-get-users/api-reference/get-friendships-incoming</para>
        /// <para>Protected accounts : https://help.twitter.com/en/safety-and-security/public-and-protected-tweets</para>
        /// </summary>
        /// <returns>An iterator to list the users who requested to follow the client's account</returns>
        IMultiLevelCursorIterator<long, IUser> GetUsersRequestingFriendship(IGetUsersRequestingFriendshipParameters parameters);

        #endregion
    }
}