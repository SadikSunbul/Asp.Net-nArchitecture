using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViabelliWebProject.Packages.Core.Persistance.Repositories;

namespace ViabelliWebProject.Packages.Core.Persistance.PageActions;

public class BasePageModel
{
    /// <summary>
    /// Tek sayfa içerisindek item sayısını belirtir
    /// </summary>
    public int Size { get; set; }
    /// <summary>
    /// Sayfa indexini belirtir 1. sayfa 0. indextir
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// Toplam item sayısını 
    /// </summary>
    public int Count { get; set; }
    /// <summary>
    /// Toplam sayfa sayısı
    /// </summary>
    public int Page { get; set; }
    /// <summary>
    /// Bu sayfanın öncesi varmı
    /// </summary>
    public bool HasPrevius => Index > 0;
    /// <summary>
    /// Bu sayfanın sonrası varmı
    /// </summary>
    public bool HasNext => Index + 1 < Page;
}
