using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Application.Request;
/// <summary>
/// Sayfalama işlemi için gerekli olan bilgileri almak için kullanılır
/// </summary>
public class PageRequest
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}