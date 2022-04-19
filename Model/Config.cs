using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CivLaucherDotNetCore
{
    public class Config
    {
        public List<Repo>? listeMod { get; set; }
        public string? apiurl { get; set; }
        public string? civModFolder { get; set; }
        public string? shortCutName { get; set; }
        public string? repoUrl { get; set; }


    }
    public class Repo
    {
        public string owner { get; set; }
        public string depot { get; set; }
    }





}
