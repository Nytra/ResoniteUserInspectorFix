using FrooxEngine;
using FrooxEngine.UIX;
using HarmonyLib;
using MonkeyLoader.Resonite;

namespace UserInspectorFix
{
    [HarmonyPatchCategory(nameof(UserInspectorFix))]
    [HarmonyPatch(typeof(UserInspector), "OnAttach")]
    internal class UserInspectorFix : ResoniteMonkey<UserInspectorFix>
    {
        public override bool CanBeDisabled => true;
        private static void Postfix(UserInspector __instance, SyncRef<Slot> ____userListContentRoot)
        {
            if (!Enabled) return;
            if (__instance.World.IsAuthority) return;
            if (____userListContentRoot.Target.ChildrenCount != 0) return; // just in case this gets fixed in the future
            foreach (var user in __instance.World.AllUsers)
            {
                __instance.RunSynchronously(delegate
                {
                    Slot slot = ____userListContentRoot.Target.AddSlot("User");
                    slot.PersistentSelf = false;
                    slot.AttachComponent<VerticalLayout>().PaddingTop.Value = 4f;
                    slot.AttachComponent<UserInspectorItem>().Setup(user);
                });
            }
        }
    }
}