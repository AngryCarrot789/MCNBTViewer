using System.Threading.Tasks;

namespace MCNBTViewer.Core.Views.Windows {
    public interface IWindow : IViewBase {
        void CloseWindow();

        Task CloseWindowAsync();
    }
}