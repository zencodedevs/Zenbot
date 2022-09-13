using System.Threading.Tasks;

namespace Zenbot.Data
{
    public interface IBirthdaysRepositoryCached : IBirthdaysRepository
    {
        public Task LoadFromSourceAsync();

        public Task SaveToSourceAsync();
    }
}
