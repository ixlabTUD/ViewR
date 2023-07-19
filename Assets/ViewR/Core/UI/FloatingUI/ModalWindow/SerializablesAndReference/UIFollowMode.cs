using ViewR.Core.UI.FloatingUI.Follower;

namespace ViewR.Core.UI.FloatingUI.ModalWindow.SerializablesAndReference
{
    public enum UIFollowMode
    {
        /// <summary>
        /// Does not change anything.
        /// </summary>
        Uncontrolled,
        /// <summary>
        /// Forces the object to follow.
        /// </summary>
        ForceFollowing,
        /// <summary>
        /// Force pins objects and does not react to <see cref="TargetFollower.ArrivedAtTargetForFirstTime"/> when reaching target for the first time.
        /// </summary>
        ForcePinned,
        /// <summary>
        /// Force pins objects and does react to <see cref="TargetFollower.ArrivedAtTargetForFirstTime"/> when reaching target for the first time.
        /// Essentially a merge of <see cref="ForcePinned"/> and <see cref="ForceFollowAndPinUponReachingTarget"/>.
        /// </summary>
        ForcePinnedAndPinUponReachingTarget,
        /// <summary>
        /// Forces the object to follow and pin upon reaching its target.
        /// </summary>
        ForceFollowAndPinUponReachingTarget,
        /// <summary>
        /// Does not change anything, but pins upon reaching its target.
        /// </summary>
        UncontrolledAndPinUponReachingTarget
    }
}