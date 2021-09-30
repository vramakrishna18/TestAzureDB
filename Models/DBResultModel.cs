using System;
using System.Collections.Generic;

public class DBInfo
{
    public string Name {get; set;}
    public string Collation_Name {get; set;}
}

public class DBResultModel
{
    public List<DBInfo> DBInfos { get; set; }    
    public DBResultModel()
    {
        DBInfos = new List<DBInfo>();
    }
  
}