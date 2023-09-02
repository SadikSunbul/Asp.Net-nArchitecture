using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Application.Piplines.Cachings;
/// <summary>
/// Appsettingsten gelicek olan default zaman ı karsılıycak sınıf
/// </summary>
public class CacheSettings
{
    /// <summary>
    /// Nekadar süre cachler tutulcak
    /// </summary>
    public int SlidingExpiration { get; set; }

}
