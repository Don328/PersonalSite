using BlazorApp.Client.Utils.Constants;
using Blazored.Modal;

namespace BlazorApp.Client.Utils
{
    public static class ModalOptionsFactory
    {

        public static ModalOptions GetOptions(ModalTypes type)
        {
            switch (type)
            {
                default:
                    return new ModalOptions();
                case ModalTypes.Default:
                    return Default();
                case ModalTypes.Scrollable:
                    return Scrollable();
            }

        }

        private static ModalOptions Default()
        {
            var opts = new ModalOptions()
            {
                Position = ModalPosition.Center,
                Animation = ModalAnimation.FadeInOut(1),
                HideHeader = true,
            };
            return opts;
        }

        private static ModalOptions Scrollable()
        {
            var opts = new ModalOptions()
            {
                Position = ModalPosition.Center,
                Animation = ModalAnimation.FadeInOut(1),
                ContentScrollable = true,
            };
            return opts;
        }
    }
}
