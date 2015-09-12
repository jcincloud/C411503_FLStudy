using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace ProcCore.Business
{
    public abstract class QueryBase
    {
        public QueryBase()
        {
        }
        public string i_Lang { get; set; }
        public int? page { get; set; }
    }
    public class SNObject
    {
        public int y { get; set; }
        public int m { get; set; }
        public int d { get; set; }
        public int w { get; set; }
        public int sn_max { get; set; }
    }
    public abstract class BaseEntityTable
    {
        public Int16 edit_type { get; set; }
        public bool check_del { get; set; }
        public bool check_sub { get; set; }
    }
}