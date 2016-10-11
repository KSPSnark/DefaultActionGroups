namespace DefaultActionGroups
{
    /// <summary>
    /// Put this module on a part in order to specify an animation to use in an action group for the part.
    /// </summary>
    public class ModuleDefaultActionGroup : PartModule
    {
        /// <summary>
        /// The module in which to find the animation to use (e.g. "ModuleAnimateGeneric")
        /// </summary>
        [KSPField]
        public string moduleSource;

        /// <summary>
        /// The GUI name of the action to work on, for toggling state (e.g. "Toggle").
        /// Ideally this should be called "toggleActionGuiName", but I already called it
        /// actionGuiName long ago and don't want to break backwards compatibility
        /// with folks who are already using the mod.
        /// 
        /// Typically, either this will be defined, or else activate/deactivate will
        /// be defined. If this field is defined, then the activate/deactivate fields
        /// will be ignored.
        /// </summary>
        [KSPField]
        public string actionGuiName;

        /// <summary>
        /// The GUI name to activate the specified action (e.g. "Activate").  If
        /// this is defined, typically deactivateGuiName will also be defined, and
        /// actionGuiName won't.
        /// </summary>
        [KSPField]
        public string activateGuiName;

        /// <summary>
        /// The GUI name to activate the specified action (e.g. "Activate").  If
        /// this is defined, typically activateGuiName will also be defined, and
        /// actionGuiName won't.
        [KSPField]
        public string deactivateGuiName;

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
    }
}
