using Nib.Exercise.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Nib.Exercise.Interfaces
{
    public interface ILocations
    {

        Task<List<Location>> GetLocations(CancellationToken cancellationToken = new CancellationToken());
    }
}
