using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.Persistance.Dynamic
{
    /// <summary>
    /// sıralama için gerekli bilgileri alamak için yazılmıstır 
    /// </summary>
    public class Sort
    {
        /// <summary>
        /// Hangi özelliğe göre bır sılralama olucak 
        /// </summary>
        public string Field { get; set; } //ozellik neye gore bır sılralama olucak 
        /// <summary>
        /// desc asc nasıl hangı turde bır sıralama olucak 
        /// </summary>
        public string Dir { get; set; } //desc asc nasıl hangı turde bır sıralama olucak 
        public Sort()
        {
            Field = string.Empty;
            Dir = string.Empty;
        }
        public Sort(string field, string dir)
        {
            Field = field;
            Dir = dir;
        }
    }
}
