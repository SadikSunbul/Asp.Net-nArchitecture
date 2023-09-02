using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Persistance.Dynamic;
/// <summary>
/// filtreleme işlemi için gereklı bilgıleri alır 
/// </summary>
public class Filter
{
    /// <summary>
    /// Hangi özelliğe göre bir filtreleme yapılcak
    /// </summary>
    public string Field { get; set; }
    /// <summary>
    /// Aranıcak ,içinde geçen,başında vb... değer
    /// </summary>
    public string? Value { get; set; } //aranacak vb deger 
    /// <summary>
    /// Özelliğin içerisinde arancakmı esıtmı olucak value değerine gibi operatörler alır
    /// </summary>
    public string? Operator { get; set; } //içinde gecen eşittir vb.
    /// <summary>
    /// 1 den fazla alanda calısma yapıcaz and or sart saglama durumları altakı ıle baglantı kurarken kullanılacak 
    /// </summary>
    public string? Logic { get; set; }//1 den fazla alanda calısma yapıcaz and or sart saglama durumları altakı ıle baglantı kurarken kullanılacak 
    /// <summary>
    /// filtre içinde filtre gibi
    /// </summary>
    public IEnumerable<Filter>? Filters { get; set; }  //filtre içinde filtre gibi
    public Filter()
    {
        Filters = new List<Filter>();
        Operator = string.Empty;
    }
    public Filter(string field, string @operator)
    {
        field = field;
        Operator = @operator;
    }
}
