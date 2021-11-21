using BlazorApp.Client.Utils.Constants;
using Blazored.Modal;

namespace BlazorApp.Client.Utils
{
    public static class ModalOptionsFactory
    {
        public static int fadeTimer = 1;

        public static ModalOptions GetOptions(ModalTypes? type)
        {
            switch (type)
            {
                case ModalTypes.Default:
                    return Default();
                case ModalTypes.Scrollable:
                    return Scrollable();
                default:
                    return Default();
            }

        }

        private static ModalOptions Default()
        {
            var opts = new ModalOptions()
            {
                Position = ModalPosition.Center,
                Animation = ModalAnimation.FadeInOut(fadeTimer),
                HideHeader = true,
                ContentScrollable = false,
                DisableBackgroundCancel = false,
                HideCloseButton = false,
            };
            return opts;
        }

        private static ModalOptions Scrollable()
        {
            var opts = new ModalOptions()
            {
                Position = ModalPosition.Center,
                Animation = ModalAnimation.FadeInOut(fadeTimer),
                ContentScrollable = true,
                HideHeader = false,
                HideCloseButton = false,
                DisableBackgroundCancel = false,

            };
            return opts;
        }
    }
}
