using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Application.Piplines.Cachings;

/// <summary>
/// Cachlerin güncel kalması için ICachable ile kalıtılmıs cachi değiştiricek yerlere verilmelidir grop key veyahutta normal cachekey i doldurulmalıdır 
/// </summary>
public interface ICacheRemoverRequest
{
    string? CacheKey { get; }
    bool BypassCache { get; }
    string? CacheGroupKey { get; }
}
