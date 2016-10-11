using System;
using UnityEngine;

namespace DefaultActionGroups
{
    /// <summary>
    /// This adds behavior to the editor so that any part with the ModuleDefaultActionGroup on it
    /// will get added to the specified action group by default.
    /// </summary>
    [KSPAddon(KSPAddon.Startup.EditorAny, false)]
    class DefaultActionGroupBehaviour : MonoBehaviour
    {
        public void Awake()
        {
            Logging.Log("Initializing");
            GameEvents.onEditorPartEvent.Add(OnPartEvent);
        }

        public void OnDestroy()
        {
            Logging.Log("Shutting down");
            GameEvents.onEditorPartEvent.Remove(OnPartEvent);
        }

        /// <summary>
        /// Handle editor events.
        /// </summary>
        /// <param name="eventType"></param>
        /// <param name="part"></param>
        private void OnPartEvent(ConstructionEventType eventType, Part part)
        {
            try
            {
                switch (eventType)
                {
                    case ConstructionEventType.PartCreated:
                        // We need this for the initial root part, since it never
                        // gets an "attached" event.
                        handlePart(part);
                        break;
                    case ConstructionEventType.PartAttached:
                        // This handles everything else, including parts that are copied, or in
                        // symmetry groups, or a child of something else that gets attached. It
                        // does have a bit of a wart in that the default groups will get re-applied
                        // every time you detach and re-attach a part... which bothered me until
                        // a bit of testing showed that the stock game does that too!
                        onPartAttached(part);
                        foreach (Part counterpart in part.symmetryCounterparts)
                        {
                            onPartAttached(counterpart);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                Logging.Exception(e);
            }
        }

        /// <summary>
        /// Called when a part is attached (either directly or as a child of something else).
        /// </summary>
        /// <param name="part"></param>
        private static void onPartAttached(Part part)
        {
            handlePart(part);
            foreach (Part child in part.children)
            {
                onPartAttached(child);
            }
        }

        /// <summary>
        /// Do the necessary processing for an individual part.
        /// </summary>
        /// <param name="part"></param>
        private static void handlePart(Part part)
        {
            foreach (ModuleDefaultActionGroup module in part.Modules.GetModules<ModuleDefaultActionGroup>())
            {
                handleDefaultGroupsForPart(part, module);
            }
        }

        /// <summary>
        /// For a part that has a default action group module defined, add it to action groups as appropriate.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="defaultActionGroupModule"></param>
        private static void handleDefaultGroupsForPart(Part part, ModuleDefaultActionGroup defaultActionGroupModule)
        {
            foreach (PartModule module in part.Modules)
            {
                if (module.moduleName == defaultActionGroupModule.moduleSource)
                {
                    handleDefaultGroupsForPartModule(part, module, defaultActionGroupModule);
                }
            }
        }

        /// <summary>
        /// Handle default action groups when we have a part that we want with a module that we want.
        /// </summary>
        /// <param name="part">The part whose action groups we need to handle.</param>
        /// <param name="module">The PartModule whose action we need to add to the appropriate action group.</param>
        /// <param name="defaultActionGroupModule">The ModuleDefaultActionGroup that tells us which action to add to which group.</param>
        private static void handleDefaultGroupsForPartModule(
            Part part,
            PartModule module,
            ModuleDefaultActionGroup defaultActionGroupModule)
        {
            ModuleAnimateGeneric animationModule = module as ModuleAnimateGeneric;
            if (animationModule != null)
            {
                // this needs special handling
                handleDefaultGroupsForAnimationModule(part, animationModule, defaultActionGroupModule);
                return;
            }

            ModuleColorChanger colorChangerModule = module as ModuleColorChanger;
            if (colorChangerModule != null)
            {
                // this also needs special handling
                handleDefaultGroupsForColorChangerModule(part, colorChangerModule, defaultActionGroupModule);
                return;
            }

            // For anything else, we use the GUI name of the action itself.
            foreach (BaseAction action in module.Actions)
            {
                if (action.guiName == defaultActionGroupModule.actionGuiName)
                {
                    Logging.Log("Updating action groups for "
                        + part.name + "/" + module.moduleName + "/" + action.guiName
                        + " to include " + defaultActionGroupModule.defaultActionGroup);
                    action.actionGroup |= defaultActionGroupModule.defaultActionGroup;
                    action.defaultActionGroup |= defaultActionGroupModule.defaultActionGroup;
                } // if the action is the one we're looking for
            } // for each action in the module
        }


        /// <summary>
        /// Handle default action groups when we have a part that we want with an animation module that we want.
        /// </summary>
        /// <param name="part">The part whose action groups we need to handle.</param>
        /// <param name="animationModule">The animation whose action we need to add to the appropriate action group.</param>
        /// <param name="defaultActionGroupModule">The ModuleDefaultActionGroup that tells us which action to add to which group.</param>
        private static void handleDefaultGroupsForAnimationModule(
            Part part,
            ModuleAnimateGeneric animationModule,
            ModuleDefaultActionGroup defaultActionGroupModule)
        {
            // ModuleAnimateGeneric needs special handling. If we just ask its actions about their guiName, they'll
            // give a name that's not actually displayed in the editor. So for this case, we ask the module itself
            // for its action GUI name.
            if (animationModule.actionGUIName == defaultActionGroupModule.actionGuiName)
            {
                foreach (BaseAction action in animationModule.Actions)
                {
                    Logging.Log("Updating action groups for animation '" + animationModule.actionGUIName + "', "
                        + part.name + "/" + animationModule.moduleName + "/" + action.name + ":" + action.guiName
                        + " to include " + defaultActionGroupModule.defaultActionGroup);
                    action.actionGroup |= defaultActionGroupModule.defaultActionGroup;
                    action.defaultActionGroup |= defaultActionGroupModule.defaultActionGroup;
                }
            }
        }

        /// <summary>
        /// Special handling for ModuleColorChanger.  Needs special handling because we can't go by the GUI
        /// name of the action, at least not if it's the root part.  Reason: that module changes the GUI name
        /// at start time, which happens *after* the part being created.
        /// </summary>
        /// <param name="part"></param>
        /// <param name="colorChangerModule"></param>
        /// <param name="defaultActionGroupModule"></param>
        private static void handleDefaultGroupsForColorChangerModule(
            Part part,
            ModuleColorChanger colorChangerModule,
            ModuleDefaultActionGroup defaultActionGroupModule)
        {
            if (colorChangerModule.toggleName != defaultActionGroupModule.actionGuiName) return;
            BaseAction toggleAction = colorChangerModule.Actions["ToggleAction"];
            Logging.Log("Updating action groups for "
                + part.name + "/" + colorChangerModule.moduleName + "/" + toggleAction.guiName
                + " to include " + defaultActionGroupModule.defaultActionGroup);
            toggleAction.actionGroup |= defaultActionGroupModule.defaultActionGroup;
            toggleAction.defaultActionGroup |= defaultActionGroupModule.defaultActionGroup;
        }
    }
}