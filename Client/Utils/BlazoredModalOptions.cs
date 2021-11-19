using Blazored.Modal;

namespace BlazorApp.Client.Utils
{
    public class BlazoredModalOptions
    {
        private readonly ModalOptions options;

        public BlazoredModalOptions(ModalOptions options)
        {
            this.options = options;
        }

        public BlazoredModalOptions(string modalType)
        {
            switch (modalType)
            {
                default:
                    break;
                case Constants.ModalTypes.About:
                    options = CreateAboutModalOptions();
                    break;
            }

        }

        private ModalOptions CreateAboutModalOptions()
        {
            var opts = new ModalOptions();
            return opts;
        }
    }
}
