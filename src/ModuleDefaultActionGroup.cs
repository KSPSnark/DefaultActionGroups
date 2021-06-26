namespace DefaultActionGroups
{
    /// <summary>
    /// Put this module on a part in order to specify an animation to use in an action group for the part.
    /// </summary>
    public class ModuleDefaultActionGroup : PartModule
    {
        /// <summary>
        /// The (non-localized) action name for ModuleAnimateGeneric's toggle action.
        /// </summary>
        private const string ANIMATION_TOGGLE_ACTION = "ToggleAction";

        /// <summary>
        /// The module in which to find the animation to use (e.g. "ModuleAnimateGeneric")
        /// </summary>
        [KSPField]
        public string moduleSource;

        /// <summary>
        /// The name of the action to work on, for toggling state (e.g. "Toggle").
        /// Ideally this should be called "toggleActionGuiName", but I already called it
        /// actionGuiName long ago and don't want to break backwards compatibility
        /// with folks who are already using the mod.
        /// 
        /// Typically, either this will be defined, or else activate/deactivate will
        /// be defined. If this field is defined, then the activate/deactivate fields
        /// will be ignored.
        /// </summary>
        [KSPField]
        public string actionName;

        /// <summary>
        /// DEPRECATED. Like actionName, but uses the GUI name of the action, instead.
        /// If both actionGuiName and actionName are specified, then actionName will be
        /// used and actionGuiName is ignored.
        /// (This is deprecated because it doesn't play nice with localization; if you
        /// use a GUI name, it will only work in one particular language.)
        /// </summary>
        [KSPField]
        public string actionGuiName;

        /// <summary>
        /// The action group (or groups) that it should be added to by default.
        /// </summary>
        [KSPField]
        public KSPActionGroup defaultActionGroup;

        /// <summary>
        /// Gets whether this module is in "toggle" mode (as opposed to activate/deactivate mode).
        /// </summary>
        public bool IsToggleMode
        {
            get
            {
                return !string.IsNullOrEmpty(actionGuiName);
            }
        }

        /// <summary>
        /// Determines whether this module matches the specified action, by name. If actionName
        /// is specified, uses that.  Otherwise, will fall back on actionGuiName.
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool MatchesActionName(BaseAction action)
        {
            if (string.IsNullOrEmpty(actionName))
            {
                return action.guiName == actionGuiName;
            }
            else
            {
                return action.name == actionName;
            }
        }

        /// <summary>
        /// Determines whether this module matches the specified animation module, testing
        /// actionName against the animation module's (non-localized) toggle action name
        /// and animationName.  If actionName isn't specified, it tests actionGuiName against
        /// the animation module's actionGUIName.
        /// </summary>
        /// <param name="animation"></param>
        /// <returns></returns>
        public bool MatchesAnimation(ModuleAnimateGeneric animation)
        {
            if (string.IsNullOrEmpty(actionName))
            {
                return animation.actionGUIName == actionGuiName;
            }
            else
            {
                return (ANIMATION_TOGGLE_ACTION == actionName) || (animation.animationName == actionName);
            }
        }
    }
}
