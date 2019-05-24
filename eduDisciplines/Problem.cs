using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htyWEBlib.eduDisciplines
{
    public class Problem
    {
        public int Id { get; set; }
        public DistributionType Distribution { get; set; }
        /// <summary>
        /// Условие задачи
        /// </summary>
        public string Condition { get; set;}
        /// <summary>
        /// График, если есть
        /// </summary>
        public string Chart { get; set; }
        /// <summary>
        /// Таблица если есть
        /// </summary>
        public object Table { get; set; }

    }
}
