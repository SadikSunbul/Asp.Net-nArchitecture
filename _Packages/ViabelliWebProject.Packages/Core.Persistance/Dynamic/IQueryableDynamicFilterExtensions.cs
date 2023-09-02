using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Persistance.Dynamic;

public static class IQueryableDynamicFilterExtensions
{

    private static readonly string[] _orders = { "asc", "desc" }; //sıralam için bunlar
    private static readonly string[] _logics = { "and", "or" }; //and ve or var sadece filtreleri aglamak ıcın

    /// <summary>
    /// Kullanabılecegımız operatorler
    /// </summary>
    private static readonly IDictionary<string, string> _operators = new Dictionary<string, string>
    {
        { "eq", "=" },
        { "neq", "!=" },
        { "lt", "<" },
        { "lte", "<=" },
        { "gt", ">" },
        { "gte", ">=" },
        { "isnull", "== null" },
        { "isnotnull", "!= null" },
        { "startswith", "StartsWith" },
        { "endswith", "EndsWith" },
        { "contains", "Contains" },
        { "doesnotcontain", "Contains" }
    };
    /// <summary>
    /// Bu methot ham sorguyu alır ve dynamicQuery den gelen verielere göre filtreleme sıralama işlemi yapar ve geri döner
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="dynamicQuery"></param>
    /// <returns></returns>
    //burada drekt olraka sorgu halındekı IQuerayable a eklentı methodu yazıyoruz 
    public static IQueryable<T> ToDynamic<T>(this IQueryable<T> query, DynamicQuery dynamicQuery)
    {
        if (dynamicQuery.Filter is not null) //fıltre var ıse ekle
            query = Filter(query, dynamicQuery.Filter);
        if (dynamicQuery.Sort is not null && dynamicQuery.Sort.Any())//sort var ıse sortlarıda ekle
            query = Sort(query, dynamicQuery.Sort);
        return query;
    }
    /// <summary>
    /// parametre olarak gelen sorguya filtreleme işlemi yapar
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="queryable"></param>
    /// <param name="filter"></param>
    /// <returns></returns>
    private static IQueryable<T> Filter<T>(IQueryable<T> queryable, Filter filter)
    {
        IList<Filter> filters = GetAllFilters(filter); //filtreleri liste haline getirdik burada 
        string?[] values = filters.Select(f => f.Value).ToArray();//filtrelerin değerlerini bir dizye aktardık 
        string where = Transform(filter, filters); //strınge donusturdu fıltreyi
        if (!string.IsNullOrEmpty(where) && values != null) //filtre ve strıng null degıl ıse gir
            queryable = queryable.Where(where, values); //filtreleem işelmi tamam

        return queryable;
    }

    private static IQueryable<T> Sort<T>(IQueryable<T> queryable, IEnumerable<Sort> sort)
    {
        foreach (Sort item in sort) //sortları gez
        {
            if (string.IsNullOrEmpty(item.Field)) //fıelad nul ıse hata 
                throw new ArgumentException("Invalid Field");
            if (string.IsNullOrEmpty(item.Dir) || !_orders.Contains(item.Dir))//sıralama nuş ve ıcermıyorsa hata 
                throw new ArgumentException("Invalid Order Type");
        }

        if (sort.Any())//sort ıcerıyor ıse git
        {
            string ordering = string.Join(separator: ",", values: sort.Select(s => $"{s.Field} {s.Dir}"));
            return queryable.OrderBy(ordering);//sırala
        }

        return queryable; //don
    }

    public static IList<Filter> GetAllFilters(Filter filter)
    {
        List<Filter> filters = new();//yenı bır filtre listesi olustur
        GetFilters(filter, filters); //ustekı listeye fıltrelerı ekleme işlemi yapılıyor burada 
        return filters; //listeyi donduk
    }

    /// <summary>
    /// gelen filtre degerine göre filtrelere listesine ekleme yapar burası
    /// filtre in içine biz gene bir filtre aldıgımız ıcın onlarıda rekursıf sekılde ekler elimizde filtre kalmayıncaya kadar boyle devam eder
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="filters"></param>
    private static void GetFilters(Filter filter, IList<Filter> filters)
    {
        filters.Add(filter); //fıltreyı ekler fıltrelere
        if (filter.Filters is not null && filter.Filters.Any()) //bu fıltreden sonra baska bır fıltre varmı dıy eontrol eder var ıse gir
            foreach (Filter item in filter.Filters) //filtrenin filtrelerini don
                GetFilters(item, filters); //rekursıf
    }
    /// <summary>
    /// filtreyı strınge donusturuyor 
    /// </summary>
    /// <param name="filter"></param>
    /// <param name="filters"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string Transform(Filter filter, IList<Filter> filters)
    {
        if (string.IsNullOrEmpty(filter.Field))//fıltrenın ozellıgı yoksa hata 
            throw new ArgumentException("Invalid Field");
        if (string.IsNullOrEmpty(filter.Operator) || !_operators.ContainsKey(filter.Operator))
            throw new ArgumentException("Invalid Operator"); //fıltrenın operatoru veya bizim operatorlerden degil ise hata 

        int index = filters.IndexOf(filter); //filtrenin indexi
        string comparison = _operators[filter.Operator]; //filterenın operatorunu al
        StringBuilder where = new(); //strıng defer olusturcaz 

        if (!string.IsNullOrEmpty(filter.Value)) //fıltrenın degerı null veya bos ıse !false olcak ve gircek bos ıse degerı
        {
            if (filter.Operator == "doesnotcontain")
                where.Append($"(!np({filter.Field}).{comparison}(@{index.ToString()}))");
            else if (comparison is "StartsWith" or "EndsWith" or "Contains")
                where.Append($"(np({filter.Field}).{comparison}(@{index.ToString()}))");
            else
                where.Append($"np({filter.Field}) {comparison} @{index.ToString()}");
        }
        else if (filter.Operator is "isnull" or "isnotnull") //operator bunlardan bırı ıse gir
        {
            where.Append($"np({filter.Field}) {comparison}");
        }

        if (filter.Logic is not null && filter.Filters is not null && filter.Filters.Any())
        {//"filter" nesnesinin "Logic" özelliği null olmadığı, "Filters" özelliği null olmadığı ve "Filters" koleksiyonunun en az bir eleman içerdiği durumları kontrol eder.
            if (!_logics.Contains(filter.Logic))//icermiyosa bu lojıgı hata 
                throw new ArgumentException("Invalid Logic");
            return $"{where} {filter.Logic} ({string.Join(separator: $" {filter.Logic} ", value: filter.Filters.Select(f => Transform(f, filters)).ToArray())})";
        }

        return where.ToString();
    }
}
