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
        /// The GUI name of the action to work on (e.g. "Toggle").
        /// </summary>
        [KSPField]
        public string actionGuiName;

        /// <summary>
        /// The action group (or groups) that it should be added to by default.
        /// </summary>
        [KSPField]
        public KSPActionGroup defaultActionGroup;
    }
}
