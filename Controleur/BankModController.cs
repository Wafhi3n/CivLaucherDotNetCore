using CivLauncher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CivLaucherDotNetCore.Controleur
{
    public class BankModController
    {
        static readonly HttpClient client = new HttpClient();
        public BankMod bm { get; set; }
        public List<ModController> modsController { get; set; }

        public BankModController(BankMod bm)
        {
            this.bm = bm;
            modsController = new List<ModController>();
        }


        internal void GetAllModsFromConfig()
        {
            foreach (Repo mod in bm.repositoriesInfo)
            { 
                Mod m = new Mod(bm.FullcivModFolder, bm.apiurl, mod);
                m.repoUrl = bm.config.repoUrl;
                bm.mods.Add(m);
                ModController mc = new ModController(m);
                m.mc = mc;
                modsController.Add(mc);
            }
        }

        public void UpdateAllModLastAviableRelease()
        {


            foreach (ModController mod in modsController)
            {
               mod.getLastTagNameReleaseFromRepo();
            }

            

        }



        public void InitialiseAllModRepoFromPath()
        {
            foreach (ModController mod in modsController)
            {
                mod.initLocalRepositoryFromExistingFolder();
            }
        }
    }
}
