using BlazorApp.Client.Utils.Enums;
using Blazored.Modal;

namespace BlazorApp.Client.Utils
{
    public static class ModalOptionsFactory
    {
        public static float fadeTimer = .5f;

        public static ModalOptions GetOptions(ModalTypes? type, AppState state)
        {
            switch (type)
            {
                case ModalTypes.Default:
                    return Default(state);
                case ModalTypes.Scrollable:
                    return Scrollable(state);
                default:
                    return Default(state);
            }

        }

        private static ModalOptions Default(AppState state)
        {
            var opts = new ModalOptions()
            {
                Position = ModalPosition.Center,
                Animation = ModalAnimation.FadeInOut(fadeTimer),
                HideHeader = false,
                ContentScrollable = false,
                DisableBackgroundCancel = false,
                HideCloseButton = false,
                Class=state.SelectedTheme,
            };
            return opts;
        }

        private static ModalOptions Scrollable(AppState state)
        {
            var opts = new ModalOptions()
            {
                Position = ModalPosition.Center,
                Animation = ModalAnimation.FadeInOut(fadeTimer),
                ContentScrollable = true,
                HideHeader = false,
                HideCloseButton = false,
                DisableBackgroundCancel = false,
                Class = state.SelectedTheme + " modal",
                OverlayCustomClass = state.SelectedTheme + "modal-overlay",
            };
            return opts;
        }
    }
}
