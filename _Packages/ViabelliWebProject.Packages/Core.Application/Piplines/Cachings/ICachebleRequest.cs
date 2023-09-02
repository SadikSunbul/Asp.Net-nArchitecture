using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Application.Piplines.Cachings;

/// <summary>
/// Caclenicek requestleri bu sınıftan kalıtılması gerekir içerisine cechkeyi verilmelidir
/// </summary>
public interface ICachebleRequest
{ 
    string CacheKey { get; }
    bool ByPassCache { get; }
    public string? CacheGroupKey { get; }
    TimeSpan? SlidiExpration { get; } //Sure nekadar kalıcak 
}
